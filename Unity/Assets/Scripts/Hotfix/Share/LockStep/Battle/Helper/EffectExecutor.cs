using System.Collections.Generic;

namespace ET
{
    public static partial class EffectExecutor
    {
        public static void Execute(int groupId, LSUnit owner, List<SearchUnit> targets)
        {
            var resEffects = TbSkillEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (var resEffect in resEffects) {
                for (var index = targets.Count - 1; index >= 0; index--) {
                    var target = targets[index];
                    Execute(resEffect, owner, target.Target, default);
                }
            }
        }
        
        public static void Execute(int groupId, LSUnit owner, LSUnit target)
        {
            var resEffects = TbSkillEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (var resEffect in resEffects) {
                Execute(resEffect, owner, target, default);
            }
        }
        
        public static void Execute(int groupId, LSUnit owner, LSUnit target, Entity carrier)
        {
            var resEffects = TbSkillEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (var resEffect in resEffects) {
                Execute(resEffect, owner, target, carrier);
            }
        }
        
        public static void ReverseExecute(int groupId, LSUnit owner, LSUnit target)
        {
            var resEffects = TbSkillEffect.Instance.GetGroupEffects(groupId);
            if (resEffects == null) {
                return;
            }
            foreach (var resEffect in resEffects) {
                ReverseExecute(resEffect, owner, target);
            }
        }

        private static void Execute(TbSkillEffectRow res, LSUnit owner, LSUnit target, Entity carrier)
        {
            if (target == null) {
                return;
            }
            switch (res.ActionType) {
                case ESkillEffectType.ADD_BUFF:
                    AddBuff(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.CHANGE_PROPERTY:
                    AddProperty(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.CHANGE_PROPERTY_DYNAMIC:
                    AddDynamicProperty(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.CHANGE_STATE:
                    ChangeState(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.ADD_RESTRICT:
                    AddRestrict(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.ADD_BULLET:
                    AddBullet(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.ATTACK:
                    DoDamage(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.RESEARCH:
                    DoResearch(res.ActionParam, owner, target, carrier);
                    break;
                case ESkillEffectType.SUMMON_SOLDIER:
                    SummonSoldier(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.GEN_DROP:
                    GenDrop(res.ActionParam, owner, target);
                    break;                
                case ESkillEffectType.ADD_SEED:
                    AddSeed(res.ActionParam, owner, target);
                    break;
                case ESkillEffectType.RANDOM_GEN_DROP:
                    RandomGenDrop(res.ActionParam, owner, target);
                    break;
            }
        }
        
        private static void ReverseExecute(TbSkillEffectRow tbEffectRow, LSUnit owner, LSUnit target)
        {
            switch (tbEffectRow.ActionType) {
                case ESkillEffectType.CHANGE_PROPERTY:
                    SubProperty(tbEffectRow.ActionParam, owner, target);
                    break;
            }
        }
        
    }
}