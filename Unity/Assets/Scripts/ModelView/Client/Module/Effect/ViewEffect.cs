using ST.Mono;
using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(ViewEffectComponent))]
    public class ViewEffect : Entity, IAwake<GameObject, bool, float>, IUpdate, IDestroy
    {
        public GameObject GameObject;
        public bool IsLoop;
        public float Speed;
        public float DurationTime;
        
        public EffectMono Mono;
    }
}