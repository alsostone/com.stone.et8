using System;

namespace ET
{
    public class EffectExecutorAttribute: BaseAttribute
    {
        public ESkillEffectType EffectType { get; }

        public EffectExecutorAttribute(ESkillEffectType type)
        {
            this.EffectType = type;
        }
    }
}