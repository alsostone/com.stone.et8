using System.Collections.Generic;

namespace ET
{
    public static class EffectExecutor
    {
        public static void Execute(int groupId, LSUnit owner, List<SearchUnit> targets)
        {
            if (targets.Count == 0) return;
            
            var resEffects = TbEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (TbEffectRow resEffect in resEffects) {
                for (int index = targets.Count - 1; index >= 0; index--) {
                    SearchUnit target = targets[index];
                    EffectExecutorComponent.Instance.Run(resEffect, owner, target.Target);
                }
            }
        }
        
        public static void Execute(int groupId, LSUnit owner, LSUnit target)
        {
            var resEffects = TbEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (TbEffectRow resEffect in resEffects) {
                EffectExecutorComponent.Instance.Run(resEffect, owner, target);
            }
        }
        
        public static void Execute(int groupId, LSUnit owner, LSUnit target, LSUnit carrier)
        {
            var resEffects = TbEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (TbEffectRow resEffect in resEffects) {
                EffectExecutorComponent.Instance.Run(resEffect, owner, target, carrier);
            }
        }
        
        public static void ReverseExecute(int groupId, LSUnit owner, LSUnit target)
        {
            var resEffects = TbEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (var resEffect in resEffects) {
                ReverseExecute(resEffect, owner, target);
            }
        }

        private static void ReverseExecute(TbEffectRow tbEffectRow, LSUnit owner, LSUnit target)
        {
            switch (tbEffectRow.ActionType) {
                case ESkillEffectType.AddProperty:
                    EffectExecutorComponent.Instance.Run(ESkillEffectType.SubProperty, tbEffectRow.ActionParam, owner, target);
                    break;
                case ESkillEffectType.AddRestrict:
                    EffectExecutorComponent.Instance.Run(ESkillEffectType.RemoveRestrict, tbEffectRow.ActionParam, owner, target);
                    break;
            }
        }
        
    }
}