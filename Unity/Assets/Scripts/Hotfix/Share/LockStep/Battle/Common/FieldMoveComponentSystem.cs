using ST.GridBuilder;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(FieldMoveComponent))]
    [FriendOf(typeof(FieldMoveComponent))]
    public static partial class FieldMoveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FieldMoveComponent self)
        {

        }
        
        public static void DoMove(this FieldMoveComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(95, self.LSParent().Id);
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();

            FieldV2 position = new FieldV2(transformComponent.Position.x, transformComponent.Position.z);
            FieldV2 v2 = gridMapComponent.GetGridData().GetFieldVector(position);
            if (v2.x == 0 && v2.z == 0)
                transformComponent.Move(new TSVector2(-position.x, -position.z));
            else
                transformComponent.Move(new TSVector2(v2.x, v2.z));
        }
        
    }
}