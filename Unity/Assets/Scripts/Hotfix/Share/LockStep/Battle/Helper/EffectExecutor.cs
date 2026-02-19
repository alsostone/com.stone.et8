using System.Collections.Generic;

namespace ET
{
    public static class EffectExecutor
    {
        public static void Execute(int groupId, LSUnit owner, List<SearchUnit> targets, int count = 1)
        {
            if (targets.Count == 0) return;
            
            var resEffects = TbEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (TbEffectRow resEffect in resEffects) {
                for (int index = targets.Count - 1; index >= 0; index--) {
                    SearchUnit target = targets[index];
                    EffectExecutorComponent.Instance.Run(resEffect, count, owner, target.Target);
                }
            }
        }
        
        public static void Execute(int groupId, LSUnit owner, LSUnit target, LSUnit carrier, int count = 1)
        {
            var resEffects = TbEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (TbEffectRow resEffect in resEffects) {
                EffectExecutorComponent.Instance.Run(resEffect, count, owner, target, carrier);
            }
        }
        
        public static void ReverseExecute(int groupId, LSUnit owner, LSUnit target, int count = 1)
        {
            var resEffects = TbEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (var resEffect in resEffects) {
                ReverseExecute(resEffect, count, owner, target);
            }
        }

        private static void ReverseExecute(TbEffectRow tbEffectRow, int count, LSUnit owner, LSUnit target)
        {
            switch (tbEffectRow.ActionType) {
                case EffectActionType.AddProperty:
                    EffectExecutorComponent.Instance.Run(EffectActionType.SubProperty, tbEffectRow.ActionParam, count, owner, target);
                    break;
                case EffectActionType.SubProperty:
                    EffectExecutorComponent.Instance.Run(EffectActionType.AddProperty, tbEffectRow.ActionParam, count, owner, target);
                    break;
                case EffectActionType.AddRestrict:
                    EffectExecutorComponent.Instance.Run(EffectActionType.RemoveRestrict, tbEffectRow.ActionParam, count, owner, target);
                    break;
                case EffectActionType.RemoveRestrict:
                    EffectExecutorComponent.Instance.Run(EffectActionType.AddRestrict, tbEffectRow.ActionParam, count, owner, target);
                    break;
                case EffectActionType.AddBuff:
                case EffectActionType.AddHeightBuff:
                    EffectExecutorComponent.Instance.Run(EffectActionType.RemoveBuff, tbEffectRow.ActionParam, count, owner, target);
                    break;
            }
        }
        
    }
}