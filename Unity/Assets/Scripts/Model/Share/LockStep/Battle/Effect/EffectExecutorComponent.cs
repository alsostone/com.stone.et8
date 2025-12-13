using System;
using System.Collections.Generic;

namespace ET
{
    [Code]
    public class EffectExecutorComponent : Singleton<EffectExecutorComponent>, ISingletonAwake
    {
        private readonly Dictionary<EffectActionType, IEffectExecutor> effectExecutors = new();
        
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
                    this.effectExecutors.Add(effectExecutorAttribute.ActionType, effectExecutor);
                }
            }
        }

        public void Run(EffectActionType type, int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
        {
            if (target == null || target.DeadMark > 0) return;
            if (effectExecutors.TryGetValue(type, out IEffectExecutor effectExecutor)) {
                effectExecutor.Run(param, count, owner, target, carrier);
            }
        }
        
        public void Run(TbEffectRow resEffect, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
        {
            if (target == null || target.DeadMark > 0) return;

            if (effectExecutors.TryGetValue(resEffect.ActionType, out IEffectExecutor effectExecutor)) {
                effectExecutor.Run(resEffect.ActionParam, count, owner, target, carrier);
            }
            if (resEffect.Fx > 0) {
                EventSystem.Instance.Publish(target.LSWorld(), new LSUnitFx() { Id = target.Id, FxId = resEffect.Fx });
            }
        }
    }
}