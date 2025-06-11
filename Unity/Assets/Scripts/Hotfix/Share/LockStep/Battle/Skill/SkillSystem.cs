using System;

namespace ET
{
    [LSEntitySystemOf(typeof(Skill))]
    [EntitySystemOf(typeof(Skill))]
    [FriendOf(typeof(Skill))]
    public static partial class SkillSystem
    {
        [EntitySystem]
        private static void Awake(this Skill self, int skillId, bool isOnlyOnce)
        {
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
        {
            // 普通攻击的CD由攻速计算
            if (self.TbSkillRow.SkillType == ESkillType.Normal) {
                var atkSpeed = self.LSOwner().GetComponent<PropComponent>().Get(NumericType.AtkSpeed);
                return self.CastFrame + ((int)(LSConstValue.Milliseconds / atkSpeed)).Convert2Frame() > self.LSWorld().Frame;
            }
            return self.CastFrame + self.TbSkillRow.CdTime.Convert2Frame() > self.LSWorld().Frame;
        }
        
        private static bool CheckReady(this Skill self)
        {
            if (self.IsRunning || self.IsInCd()) { return false; }
            
            // if (!ConditionCheck.CheckCondition(mEntity.Handle, ResSkill)) {
            //     return false;
            // }
            return true;
        }

        private static int SearchTargets(this Skill self)
        {
            self.SearchUnits.Clear();
            TargetSearcher.Search(self.TbSkillRow.SearchTarget, self.LSOwner(), self.SearchUnits);
            return self.SearchUnits.Count;
        }
        
        public static bool TryCast(this Skill self)
        {
            if (!self.CheckReady()) { return false; }
            if (!self.TbSkillRow.SearchRealTime && self.SearchTargets() == 0) { return false; }
            
            if (LSConstValue.Probability > self.TbSkillRow.Probability) {
                if (self.GetRandom().Range(0, LSConstValue.Probability) >= self.TbSkillRow.Probability) {
                    return false;
                }
            }

            // mEntity.ComActorPathfinding?.QuitPathfinding();
            // if (!string.IsNullOrEmpty(ResSkill.ani_name)) {
            //     mEntity.ComState?.ChangeState(StateType.Skill, ResSkill.id_key);
            // }
            
            self.IsRunning = true;
            self.CastFrame = self.LSWorld().Frame;
            self.StepRunning();
            return true;
        }
        
        public static void ForceDone(this Skill self)
        {
            self.IsRunning = false;
            self.CurrentPoint = 0;
            self.SearchUnits.Clear();
            
            if (self.IsOnlyOnce) {
                self.Dispose();
            }
        }

        private static void OnCastSuccess(this Skill self)
        {
            var tbRow = self.TbSkillRow;
            switch (tbRow.ConsumeType) {
                case EConsumeType.Property:
                    if (tbRow.ConsumeParam.Length != 2) { return; }
                    self.LSOwner().GetComponent<PropComponent>().Add((NumericType)tbRow.ConsumeParam[0], -tbRow.ConsumeParam[1]);
                    break;
            }
        }

        private static void OnCastDone(this Skill self)
        {
            self.IsRunning = false;
            self.CurrentPoint = 0;
            self.SearchUnits.Clear();

            // if (!string.IsNullOrEmpty(ResSkill.ani_name)) {
            //     mEntity.ComState?.ChangeState(StateType.Idle);
            // }
            if (self.IsOnlyOnce) {
                self.Dispose();
            }
        }

        public static void StepRunning(this Skill self)
        {
            if (self.CastFrame + self.DurationFrame > self.LSWorld().Frame)
            {
                // 到达效果触发点后触发效果
                for (var index = self.CurrentPoint; index < self.TbSkillRow.TriggerArray.Length; index++)
                {
                    var frame = self.TbSkillRow.TriggerArray[index].Convert2Frame();
                    if (self.LSWorld().Frame - self.CastFrame >= frame)
                    {
                        self.CurrentPoint = index + 1;
                        if (self.TbSkillRow.SearchRealTime && self.SearchTargets() == 0) { break; } 
                        
                        if (index == 0) { self.OnCastSuccess(); }
                        EffectExecutor.Execute(self.TbSkillRow.EffectGroupId, self.LSOwner(), self.SearchUnits);
                    }
                }
            }
            else {
                // 技能持续时间完毕 把未触发的点全部触发
                for (var index = self.CurrentPoint; index < self.TbSkillRow.TriggerArray.Length; index++)
                {
                    if (self.TbSkillRow.SearchRealTime && self.SearchTargets() == 0) { break; } 
                    
                    if (index == 0) { self.OnCastSuccess(); }
                    EffectExecutor.Execute(self.TbSkillRow.EffectGroupId, self.LSOwner(), self.SearchUnits);
                }
                self.OnCastDone();
            }
        }

    }
}