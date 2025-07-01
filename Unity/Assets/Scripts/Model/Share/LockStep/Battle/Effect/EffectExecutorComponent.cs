using System;
using System.Collections.Generic;

namespace ET
{
    [Code]
    public class EffectExecutorComponent: Singleton<EffectExecutorComponent>, ISingletonAwake
    {
        private readonly Dictionary<ESkillEffectType, IEffectExecutor> effectExecutors = new();
        
        public void Awake()
        {
            HashSet<Type> types = CodeTypes.Instance.GetTypes(typeof(EffectExecutorAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(EffectExecutorAttribute), false);

                foreach (object attr in attrs)
                {
                    EffectExecutorAttribute effectExecutorAttribute = (EffectExecutorAttribute)attr;
                    IEffectExecutor effectExecutor = Activator.CreateInstance(type) as IEffectExecutor;
                    this.effectExecutors.Add(effectExecutorAttribute.EffectType, effectExecutor);
                }
            }
        }

        public void Run(ESkillEffectType type, int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
        {
            if (target == null) return;
            if (effectExecutors.TryGetValue(type, out IEffectExecutor effectExecutor))
            {
                effectExecutor.Run(param, owner, target, carrier);
            }
        }
    }
}