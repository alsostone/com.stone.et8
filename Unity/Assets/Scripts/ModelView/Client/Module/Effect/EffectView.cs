using ST.Mono;
using UnityEngine;

namespace ET.Client
{
    [ChildOf(typeof(EffectViewComponent))]
    public class EffectView : Entity, IAwake<GameObject>, IUpdate, IDestroy
    {
        public GameObject GameObject;
        public bool IsLoop;
        public float Speed;
        public float DurationTime;
        
        public EffectMono Mono;
    }
}