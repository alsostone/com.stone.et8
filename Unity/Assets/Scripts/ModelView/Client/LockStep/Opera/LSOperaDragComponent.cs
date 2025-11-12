using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Room))]
    public class LSOperaDragComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public float raycastDistance = 100.0f;
        public LayerMask terrainMask = 1 << LayerMask.NameToLayer("Map");
        
        public Vector3 mousePosition;
        public bool isOutsideDraging = false;
        public bool isMouseDraging = false;
        public bool isKeyADown = false;
        
        public bool isDraging { get; set; } = false;
        public int dragFingerId = -1;
        
        public ETCancellationToken longTouchPreesToken;
        public EntityRef<LSUnitView> HoverUnitView;
    }
}