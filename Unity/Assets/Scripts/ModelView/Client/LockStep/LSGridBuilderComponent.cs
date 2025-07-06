using ST.GridBuilder;
using UnityEngine;

namespace ET
{
    [ComponentOf(typeof(Room))]
    public class LSGridBuilderComponent: Entity, IAwake, IUpdate
    {
        public float raycastDistance = 1000.0f;
        public GridMap gridMap;
        public GridMapIndicator gridMapIndicator;
        
        public bool isNewBuilding;
        public Placement dragPlacement;
        public Vector3 dragOffset;
        public int dragFingerId = -1;
    }
}