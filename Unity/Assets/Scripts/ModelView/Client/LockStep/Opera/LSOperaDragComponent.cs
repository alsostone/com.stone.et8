using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Room))]
    public class LSOperaDragComponent: Entity, IAwake, IUpdate, IDestroy, ILSRollback
    {
        public float raycastDistance = 100.0f;
        public LayerMask terrainMask = 1 << LayerMask.NameToLayer("Map");
        public LayerMask terrainMask2 = LayerMask.GetMask("Map", "Obstacle", "Block");
        
        public OperationMode operationMode = OperationMode.None;
        public LineRenderer lineShooting;
        
        public Vector3 mousePosition;
        public bool isOutsideDraging = false;
        public bool isItemClicked = false;
        public bool isMouseDraging = false;
        public bool isKeyADown = false;
        
        public bool isDragging = false;
        public int dragFingerId = -1;
        
        public ETCancellationToken longTouchPreesToken;
        public EntityRef<LSUnitView> HoverUnitView;
    }
}