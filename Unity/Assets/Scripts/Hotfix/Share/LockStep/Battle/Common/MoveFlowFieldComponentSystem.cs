using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(MoveFlowFieldComponent))]
    [FriendOf(typeof(MoveFlowFieldComponent))]
    public static partial class MoveFlowFieldComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MoveFlowFieldComponent self)
        {

        }
        
        public static void DoMove(this MoveFlowFieldComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(95, self.LSParent().Id);
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            
            TSVector vector = gridMapComponent.GetFieldVector(transformComponent.Position);
            if (vector.x == 0 && vector.z == 0)
                vector = TSVector.zero - transformComponent.Position;
            transformComponent.RVOMove(new TSVector2(vector.x, vector.z));
        }
        
    }
}