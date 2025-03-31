using System;
using System.Collections.Generic;

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
            self.Duration = self.TbSkillRow.DurationPre + self.TbSkillRow.Duration + self.TbSkillRow.DurationAfter;
        }

        [EntitySystem]
        private static void Destroy(this Skill self)
        {
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this Skill self)
        {
            self.StepRunning();
        }

        public static bool IsInCd(this Skill self)
        {
            // 普通攻击的CD由攻速计算
            if (self.TbSkillRow.SkillType == ESkillType.Normal) {
                var atkSpeed = self.Owner.GetComponent<PropComponent>().GetByKey(NumericType.AtkSpeed);
                return self.CastTime + (BattleConst.AtkSpeedFactor / atkSpeed) > TimeInfo.Instance.ServerNow();
            }
            return self.CastTime + self.TbSkillRow.CdTime > TimeInfo.Instance.ServerNow();
        }
        
        private static bool CheckReady(this Skill self)
        {
            if (self.IsInCd()) { return false; }
            
            // if (!ConditionCheck.CheckCondition(mEntity.Handle, ResSkill)) {
            //     return false;
            // }
            return true;
        }

        private static int SearchTargets(this Skill self)
        {
            foreach (var target in self.SearchUnits) {
                ObjectPool.Instance.Recycle(target);
            }
            self.SearchUnits.Clear();
            TargetSearcher.Search(self.TbSkillRow.SearchTarget, self.Owner, self.SearchUnits);
            return self.SearchUnits.Count;
        }
        
        public static bool TryCast(this Skill self)
        {
            if (!self.CheckReady()) { return false; }
            if (!self.TbSkillRow.SearchRealTime && self.SearchTargets() == 0) { return false; }
            
            if (BattleConst.Probability > self.TbSkillRow.Probability) {
                if (self.GetRandom().Range(0, BattleConst.Probability) >= self.TbSkillRow.Probability) {
                    return false;
                }
            }

            // mEntity.ComActorPathfinding?.QuitPathfinding();
            // if (!string.IsNullOrEmpty(ResSkill.ani_name)) {
            //     mEntity.ComState?.ChangeState(StateType.Skill, ResSkill.id_key);
            // }
            
            self.CastTime = TimeInfo.Instance.ServerNow();
            self.StepRunning();
            return true;
        }
        
        public static void ForceDone(this Skill self)
        {
            self.CurrentPoint = 0;
            
            foreach (var target in self.SearchUnits) {
                ObjectPool.Instance.Recycle(target);
            }
            self.SearchUnits.Clear();
            if (self.IsOnlyOnce) {
                self.Dispose();
            }
        }

        private static void OnCastSuccess(this Skill self)
        {
            var tbRow = self.TbSkillRow;
            switch (tbRow.ConsumeType) {
                case EConsumeType.PROPERTY:
                    if (tbRow.ConsumeParam.Length != 2) { return; }
                    self.Owner.GetComponent<PropComponent>().Add((NumericType)tbRow.ConsumeParam[0], -tbRow.ConsumeParam[1]);
                    break;
            }
        }

        private static void OnCastDone(this Skill self)
        {
            self.CurrentPoint = 0;
            
            foreach (var target in self.SearchUnits) {
                ObjectPool.Instance.Recycle(target);
            }
            self.SearchUnits.Clear();

            // if (!string.IsNullOrEmpty(ResSkill.ani_name)) {
            //     mEntity.ComState?.ChangeState(StateType.Idle);
            // }
            if (self.IsOnlyOnce) {
                self.Dispose();
            }
        }
        
        private static void StepRunning(this Skill self)
        {
            if (self.CastTime + self.Duration > TimeInfo.Instance.ServerNow())
            {
                // 到达效果触发点后触发效果
                for (var index = self.CurrentPoint; index < self.TbSkillRow.TriggerArray.Length; index++)
                {
                    var point = self.TbSkillRow.TriggerArray[index];
                    if (TimeInfo.Instance.ServerNow() - self.CastTime >= point)
                    {
                        self.CurrentPoint = index + 1;
                        if (self.TbSkillRow.SearchRealTime && self.SearchTargets() == 0) { continue; } 
                        
                        if (index == 0) { self.OnCastSuccess(); }
                        EffectExecutor.Execute(self.TbSkillRow.EffectGroupId, self.Owner, self.SearchUnits);
                    }
                }
            }
            else {
                // 技能持续时间完毕 把未触发的点全部触发
                for (var index = self.CurrentPoint; index < self.TbSkillRow.TriggerArray.Length; index++)
                {
                    if (self.TbSkillRow.SearchRealTime && self.SearchTargets() == 0) { continue; } 
                    
                    if (index == 0) { self.OnCastSuccess(); }
                    EffectExecutor.Execute(self.TbSkillRow.EffectGroupId, self.Owner, self.SearchUnits);
                }
                self.OnCastDone();
            }
        }

    }
}