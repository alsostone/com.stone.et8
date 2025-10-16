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
            
            TSVector vector = gridMapComponent.GetFieldVector(transformComponent.Position);
            if (vector.x == 0 && vector.z == 0)
                transformComponent.RVOMove(new TSVector2(-transformComponent.Position.x, -transformComponent.Position.z));
            else
                transformComponent.RVOMove(new TSVector2(vector.x, vector.z));
        }
        
    }
}