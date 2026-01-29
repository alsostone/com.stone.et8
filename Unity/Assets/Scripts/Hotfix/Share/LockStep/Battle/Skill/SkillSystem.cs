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
        private static void Awake(this Skill self, int skillId, bool isRemoveOnDone)
        {self.LSRoom()?.ProcessLog.LogFunction(133, self.LSParent().Id, skillId, isRemoveOnDone ? 1 : 0);
            self.SearchUnits = new List<SearchUnitPackable>();
            self.SkillId = skillId;
            self.IsRemoveOnDone = isRemoveOnDone;
            self.StartTime = FP.MinValue;
            self.DurationTime = (self.TbSkillRow.DurationPre + self.TbSkillRow.Duration + self.TbSkillRow.DurationAfter) * FP.EN3;
        }

        public static void RemoveSelf(this Skill self, bool forceRemove = false)
        {
            if (self.IsRunning)
            {
                self.IsRemoveOnDone = true;
                if (forceRemove) {
                    self.ForceDone();
                }
            }
            else
            {
                // 未在释放中的技能加入到移除列表（可能正在遍历所以不能直接Dispose）
                SkillComponent skillComponent = self.Parent as SkillComponent;
                skillComponent.RemovedSkills ??= ObjectPool.Instance.Fetch<List<long>>();
                skillComponent.RemovedSkills.Add(self.Id);
            }
        }
        
        public static bool IsInCd(this Skill self)
        {
            // 普通攻击的CD由攻速计算
            if (self.TbSkillRow.SkillType == ESkillType.Normal) {
                var atkSpeed = self.LSOwner().GetComponent<PropComponent>().Get(NumericType.AtkSpeed);
                return self.StartTime + FP.One / atkSpeed > self.LSWorld().ElapsedTime;
            }
            return self.StartTime + self.TbSkillRow.CdTime * FP.EN3 > self.LSWorld().ElapsedTime;
        }
        
        private static bool CheckReady(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(132, self.LSParent().Id);
            if (self.IsRunning || self.IsInCd()) { return false; }
            
            // if (!ConditionCheck.CheckCondition(mEntity.Handle, ResSkill)) {
            //     return false;
            // }
            return true;
        }

        private static int SearchTargets(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(131, self.LSParent().Id);
            self.SearchUnits.Clear();
            List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
            TargetSearcher.Search(self.TbSkillRow.SearchTarget, self.LSOwner(), targets);
            foreach (SearchUnit target in targets)
            {
                self.SearchUnits.Add(new SearchUnitPackable
                {
                    Target = target.Target.Id,
                    SqrDistance = target.SqrDistance
                });
            }
            if (targets.Count > 0) {
                self.LSOwner().GetComponent<TransformComponent>().LookAt(targets[0].Target);
            }
            targets.Clear();
            ObjectPool.Instance.Recycle(targets);
            return self.SearchUnits.Count;
        }
        
        public static bool TryCast(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(130, self.LSParent().Id);
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
            self.StartTime = self.LSWorld().ElapsedTime;
            self.StepRunning();
            
            // 持续时间大于0时，说明技能有动作
            // 释放有动作的技能时 禁止普攻&移动
            if (self.DurationTime > 0) {
                var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
                flagComponent.AddRestrict((int)(FlagRestrict.NotAttack | FlagRestrict.NotActive | FlagRestrict.NotMove));
                
                // 通知表现层播放动作
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitCasting() { Id = self.LSOwner().Id, SkillId = self.SkillId });
            }
            return true;
        }
        
        public static void ForceDone(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(129, self.LSParent().Id);
            if (!self.IsRunning) {
                return;
            }
            self.IsRunning = false;
            self.CurrentPoint = 0;
            self.SearchUnits.Clear();
            
            if (self.DurationTime > 0) {
                var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
                flagComponent.RemoveRestrict((int)(FlagRestrict.NotAttack | FlagRestrict.NotActive | FlagRestrict.NotMove));
            }
            
            if (self.IsRemoveOnDone) {
                self.RemoveSelf();
            }
        }

        private static void OnCastSuccess(this Skill self)
        {
        }
        
        private static bool TrySkillConsume(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(128, self.LSParent().Id);
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

                    TeamType team = self.LSOwner().GetComponent<TeamComponent>().GetOwnerTeam();
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
                        FP value = (FP)tbRow.ConsumeParam[i + 1] / LSConstValue.PropValueScale;
                        propComponent.Add(type, -value);
                    }
                    break;
                }
            }

            return true;
        }

        private static void OnCastDone(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(127, self.LSParent().Id);
            self.IsRunning = false;
            self.CurrentPoint = 0;
            self.SearchUnits.Clear();

            if (self.DurationTime > 0) {
                var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
                flagComponent.RemoveRestrict((int)(FlagRestrict.NotAttack | FlagRestrict.NotActive | FlagRestrict.NotMove));
            }
            
            if (self.IsRemoveOnDone) {
                self.RemoveSelf();
            }
        }

        public static void StepRunning(this Skill self)
        {self.LSRoom()?.ProcessLog.LogFunction(126, self.LSParent().Id);
            if (self.StartTime + self.DurationTime > self.LSWorld().ElapsedTime)
            {
                // 到达效果触发点后触发效果
                for (var index = self.CurrentPoint; index < self.TbSkillRow.TriggerArray.Length; index++)
                {
                    FP time = self.StartTime + self.TbSkillRow.TriggerArray[index] * FP.EN3;
                    if (self.LSWorld().ElapsedTime >= time)
                    {
                        self.CurrentPoint = index + 1;
                        
                        List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
                        if (self.TbSkillRow.SearchRealTime) {
                            TargetSearcher.Search(self.TbSkillRow.SearchTarget, self.LSOwner(), targets);
                        } else {
                            foreach (SearchUnitPackable searchUnit in self.SearchUnits) {
                                targets.Add(new SearchUnit { Target = self.LSUnit(searchUnit.Target), SqrDistance = searchUnit.SqrDistance });
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
                            targets.Add(new SearchUnit { Target = self.LSUnit(searchUnit.Target), SqrDistance = searchUnit.SqrDistance });
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