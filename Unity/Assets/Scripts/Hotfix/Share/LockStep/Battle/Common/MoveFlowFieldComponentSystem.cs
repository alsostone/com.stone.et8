using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(MoveFlowFieldComponent))]
    [EntitySystemOf(typeof(MoveFlowFieldComponent))]
    [FriendOf(typeof(MoveFlowFieldComponent))]
    public static partial class MoveFlowFieldComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MoveFlowFieldComponent self)
        {

        }
        
        [LSEntitySystem]
        private static void LSUpdate(this MoveFlowFieldComponent self)
        {
            if (self.FlowFieldStatus != FlowFieldStatus.Moving)
                return;
            
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            if (TSVector.SqrDistance(transformComponent.Position, self.FlowFieldDestination) <= FP.EN2)
            {
                self.SetArrived();
                return;
            }

            TSVector vector = gridMapComponent.GetFieldVector(self.FlowFieldIndex, transformComponent.Position);
            if (vector.sqrMagnitude <= FP.EN2)
            {
                self.SetArrived();
                return;
            }
            transformComponent.RVOMove(new TSVector2(vector.x, vector.z));
        }
        
        [EntitySystem]
        private static void Destroy(this MoveFlowFieldComponent self)
        {
            self.Stop();
        }
        
        public static void Stop(this MoveFlowFieldComponent self)
        {
            if (self.FlowFieldStatus == FlowFieldStatus.Moving)
            {
                LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
                gridMapComponent.RemoveFlowFieldReference(self.FlowFieldIndex);
                if (self.MovementMode == MovementMode.Move) {
                    self.LSOwner().GetComponent<FlagComponent>().RemoveRestrict((int)FlagRestrict.NotAIAlert);
                }
                self.FlowFieldStatus = FlowFieldStatus.None;
            }
        }
        
        private static void SetArrived(this MoveFlowFieldComponent self)
        {
            if (self.FlowFieldStatus == FlowFieldStatus.Moving)
            {
                LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
                gridMapComponent.RemoveFlowFieldReference(self.FlowFieldIndex);
                if (self.MovementMode == MovementMode.Move) {
                    self.LSOwner().GetComponent<FlagComponent>().RemoveRestrict((int)FlagRestrict.NotAIAlert);
                }
            }
            self.FlowFieldStatus = FlowFieldStatus.Arrived;
        }
        
        public static bool TryMoveStart(this MoveFlowFieldComponent self, int flowFieldIndex, TSVector destination, MovementMode movementMode = MovementMode.Move)
        {
            if (self.FlowFieldStatus != FlowFieldStatus.None)
                return false;
            
            self.MoveStart(flowFieldIndex, destination, movementMode);
            return true;
        }
        
        public static void MoveStart(this MoveFlowFieldComponent self, int flowFieldIndex, TSVector destination, MovementMode movementMode = MovementMode.Move)
        {
            self.FlowFieldIndex = flowFieldIndex;
            self.FlowFieldDestination = destination;
            self.FlowFieldStatus = FlowFieldStatus.Moving;
            self.MovementMode = movementMode;
            
            if (movementMode == MovementMode.Move) {
                self.LSOwner().GetComponent<FlagComponent>().AddRestrict((int)FlagRestrict.NotAIAlert);
            }
        }
        
        public static bool IsArrived(this MoveFlowFieldComponent self)
        {
            return self.FlowFieldStatus == FlowFieldStatus.Arrived;
        }
    }
}