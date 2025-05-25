using UnityEngine;

namespace ET
{
    [ChildOf(typeof(LSUnitViewComponent))]
    public class LSUnitView: Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }
        public EntityRef<LSUnit> Unit;
    }
}