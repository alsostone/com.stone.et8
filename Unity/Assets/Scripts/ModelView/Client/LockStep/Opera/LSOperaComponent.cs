using UnityEngine;

namespace ET
{
    [ComponentOf(typeof(Room))]
    public class LSOperaComponent: Entity, IAwake, IUpdate
    {
        public Vector2 Axis;
    }
}