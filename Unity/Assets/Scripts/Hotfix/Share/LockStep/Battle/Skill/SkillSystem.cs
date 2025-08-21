using System;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(Skill))]
    [EntitySystemOf(typeof(Skill))]
    [FriendOf(typeof(Skill))]
    public static partial class SkillSystem
    {
        [EntitySystem]
        private static void Awake(this Skill self, int skillId, bool isOnlyOnce)
        {self.LSRoom()?.ProcessLog.LogFunction(63, self.LSParent().Id, skillId, isOnlyOnce ? 1 : 0);
            self.SkillId = skillId;
            self.IsOnlyOnce = isOnlyOnce;
            self.CastFrame = int.MinValue;
            self.DurationFrame = (self.TbSkillRow.DurationPre + self.TbSkillRow.Duration + self.TbSkillRow.DurationAfter).Convert2Frame();
        }

        [EntitySystem]
        private static void Destroy(this Skill self)
        {
        }
        
        public static bool IsInCd(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(62, self.LSParent().Id);
            // 普通攻击的CD由攻速计算
            if (self.TbSkillRow.SkillType == ESkillType.Normal) {
                var atkSpeed = self.LSOwner().GetComponent<PropComponent>().Get(NumericType.AtkSpeed);
                return self.CastFrame + ((int)(LSConstValue.Milliseconds / atkSpeed)).Convert2Frame() > self.LSWorld().Frame;
            }
            return self.CastFrame + self.TbSkillRow.CdTime.Convert2Frame() > self.LSWorld().Frame;
        }
        
        private static bool CheckReady(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(61, self.LSParent().Id);
            if (self.IsRunning || self.IsInCd()) { return false; }
            
            // if (!ConditionCheck.CheckCondition(mEntity.Handle, ResSkill)) {
            //     return false;
            // }
            return true;
        }

        private static int SearchTargets(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(60, self.LSParent().Id);
            self.SearchUnits.Clear();
            List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
            TargetSearcher.Search(self.TbSkillRow.SearchTarget, self.LSOwner(), targets);
            foreach (SearchUnit target in targets)
            {
                self.SearchUnits.Add(new SearchUnitPackable
                {
                    Target = target.Target.Id,
                    Distance = target.Distance
                });
            }
            targets.Clear();
            ObjectPool.Instance.Recycle(targets);
            return self.SearchUnits.Count;
        }
        
        public static bool TryCast(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(59, self.LSParent().Id);
            if (!self.CheckReady()) { return false; }
            if (!self.TbSkillRow.SearchRealTime && self.SearchTargets() == 0) { return false; }
            
            if (LSConstValue.Probability > self.TbSkillRow.Probability) {
                if (self.GetRandom().Range(0, LSConstValue.Probability) >= self.TbSkillRow.Probability) {
                    return false;
                }
            }
            
            if (!self.TrySkillConsume()) {
                return false;
            }

            self.IsRunning = true;
            self.CastFrame = self.LSWorld().Frame;
            self.StepRunning();
            
            // 持续时间大于0时，说明技能有动作
            // 释放有动作的技能时 禁止普攻&移动
            if (self.DurationFrame > 0) {
                var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
                flagComponent.AddRestrict((int)(FlagRestrict.NotAttack | FlagRestrict.NotActive | FlagRestrict.NotMove));
                
                // 通知表现层播放动作
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitCasting() { Id = self.LSOwner().Id, SkillId = self.SkillId });
            }
            return true;
        }
        
        public static void ForceDone(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(58, self.LSParent().Id);
            if (!self.IsRunning) {
                return;
            }
            self.IsRunning = false;
            self.CurrentPoint = 0;
            self.SearchUnits.Clear();
            
            if (self.DurationFrame > 0) {
                var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
                flagComponent.RemoveRestrict((int)(FlagRestrict.NotAttack | FlagRestrict.NotActive | FlagRestrict.NotMove));
            }
            
            if (self.IsOnlyOnce) {
                self.Dispose();
            }
        }

        private static void OnCastSuccess(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(57, self.LSParent().Id);
        }
        
        private static bool TrySkillConsume(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(106, self.LSParent().Id);
            var tbRow = self.TbSkillRow;
            switch (tbRow.ConsumeType)
            {
                case EConsumeType.Property:
                {
                    if (tbRow.ConsumeParam.Length < 2)
                        return true;

                    PropComponent propComponent = self.LSOwner().GetComponent<PropComponent>();
                    for (int i = 0; i < tbRow.ConsumeParam.Length; i += 2)
                    {
                        NumericType type = (NumericType)tbRow.ConsumeParam[i];
                        FP value = tbRow.ConsumeParam[i + 1] * FP.EN4;
                        FP exsit = propComponent.Get(type);
                        if (value > 0 && exsit < (value - FP.Epsilon))
                            return false;
                    }
                    for (int i = 0; i < tbRow.ConsumeParam.Length; i += 2)
                    {
                        NumericType type = (NumericType)tbRow.ConsumeParam[i];
                        FP value = (FP)tbRow.ConsumeParam[i + 1] / LSConstValue.PropValueScale;
                        propComponent.Add(type, -value);
                    }
                    break;
                }
                case EConsumeType.TeamProperty:
                {
                    if (tbRow.ConsumeParam.Length < 2)
                        return true;

                    TeamType team = self.LSOwner().GetComponent<TeamComponent>()?.GetFriendTeam() ?? TeamType.None;
                    PropComponent propComponent = self.LSTeamUnit(team).GetComponent<PropComponent>();
                    
                    for (int i = 0; i < tbRow.ConsumeParam.Length; i += 2)
                    {
                        NumericType type = (NumericType)tbRow.ConsumeParam[i];
                        FP value = tbRow.ConsumeParam[i + 1] * FP.EN4;
                        FP exsit = propComponent.Get(type);
                        if (value > 0 && exsit < (value - FP.Epsilon))
                            return false;
                    }
                    for (int i = 0; i < tbRow.ConsumeParam.Length; i += 2)
                    {
                        NumericType type = (NumericType)tbRow.ConsumeParam[i];
                        FP value = (FP)tbRow.ConsumeParam[i + 1] / LSConstValue.PropValueScale;;
                        propComponent.Add(type, -value);
                    }
                    break;
                }
            }

            return true;
        }

        private static void OnCastDone(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(56, self.LSParent().Id);
            self.IsRunning = false;
            self.CurrentPoint = 0;
            self.SearchUnits.Clear();

            if (self.DurationFrame > 0) {
                var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
                flagComponent.RemoveRestrict((int)(FlagRestrict.NotAttack | FlagRestrict.NotActive | FlagRestrict.NotMove));
            }
            
            if (self.IsOnlyOnce) {
                self.Dispose();
            }
        }

        public static void StepRunning(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(55, self.LSParent().Id);
            if (self.CastFrame + self.DurationFrame > self.LSWorld().Frame)
            {
                // 到达效果触发点后触发效果
                for (var index = self.CurrentPoint; index < self.TbSkillRow.TriggerArray.Length; index++)
                {
                    var frame = self.TbSkillRow.TriggerArray[index].Convert2Frame();
                    if (self.LSWorld().Frame - self.CastFrame >= frame)
                    {
                        self.CurrentPoint = index + 1;
                        
                        List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
                        if (self.TbSkillRow.SearchRealTime) {
                            TargetSearcher.Search(self.TbSkillRow.SearchTarget, self.LSOwner(), targets);
                        } else {
                            foreach (SearchUnitPackable searchUnit in self.SearchUnits) {
                                targets.Add(new SearchUnit { Target = self.LSUnit(searchUnit.Target), Distance = searchUnit.Distance });
                            }
                        }
                        if (index == 0) { self.OnCastSuccess(); }
                        EffectExecutor.Execute(self.TbSkillRow.EffectGroupId, self.LSOwner(), targets);
                        targets.Clear();
                        ObjectPool.Instance.Recycle(targets);
                    }
                }
            }
            else
            {
                // 技能持续时间完毕 把未触发的点全部触发
                var count = Math.Max(self.TbSkillRow.TriggerArray.Length, 1);
                for (var index = self.CurrentPoint; index < count; index++)
                {
                    List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
                    if (self.TbSkillRow.SearchRealTime) {
                        TargetSearcher.Search(self.TbSkillRow.SearchTarget, self.LSOwner(), targets);
                    } else {
                        foreach (SearchUnitPackable searchUnit in self.SearchUnits) {
                            targets.Add(new SearchUnit { Target = self.LSUnit(searchUnit.Target), Distance = searchUnit.Distance });
                        }
                    }
                    if (index == 0) { self.OnCastSuccess(); }
                    EffectExecutor.Execute(self.TbSkillRow.EffectGroupId, self.LSOwner(), targets);
                    targets.Clear();
                    ObjectPool.Instance.Recycle(targets);
                }
                self.OnCastDone();
            }
        }

    }
}