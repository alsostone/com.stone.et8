using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Room))]
    public class LSOperaDragComponent: Entity, IAwake, IUpdate
    {
        public float raycastDistance = 100.0f;
        public LayerMask terrainMask = 1 << LayerMask.NameToLayer("Map");

        public bool isOutsideDraging = false;
        public bool isMouseDraging = false;
        
        public bool isDraging = false;
        public int dragFingerId = -1;
    }
}