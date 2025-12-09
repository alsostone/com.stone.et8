using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(TransformComponent))]
    [EntitySystemOf(typeof(TransformComponent))]
    [FriendOf(typeof(TransformComponent))]
    public static partial class TransformComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TransformComponent self, TSVector position, TSQuaternion rotation)
        {self.LSRoom()?.ProcessLog.LogFunction(94, self.LSParent().Id, position.x.V, position.y.V, position.z.V);
            self.Position = position;
            self.Rotation = rotation;
            self.Upwards = rotation * TSVector.up;
        }

        [LSEntitySystem]
        private static void LSUpdate(this TransformComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(93, self.LSParent().Id);
            if (self.IsMovingCurrent) {
                self.IsMovingCurrent = false;
            }
            else if (self.IsMovingPrevious) {
                self.SetMoving(false);
            }
        }
        
        public static void Move(this TransformComponent self, TSVector2 forward)
        {self.LSRoom()?.ProcessLog.LogFunction(92, self.LSParent().Id, forward.x.V, forward.y.V);
            if (forward.sqrMagnitude < FP.EN4) {
                self.SetMoving(false);
                return;
            }
            FlagComponent flagComponent = self.LSOwner().GetComponent<FlagComponent>();
            if (flagComponent.HasRestrict(FlagRestrict.NotMove)) {
                self.SetMoving(false);
                return;
            }
            
            self.Forward = new TSVector(forward.x, 0, forward.y);
            self.SetMoving(true);
            
            PropComponent propComponent = self.LSOwner().GetComponent<PropComponent>();
            TSVector2 v2 = forward.normalized * propComponent.Get(NumericType.Speed) * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            if (v2.LengthSquared() > FP.EN4)
            {
                self.Position += new TSVector(v2.x, 0, v2.y);
                LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                rvo2Component.SetAgentPosition(self.LSOwner(), new TSVector2(self.Position.x, self.Position.z));
            }
        }
        
        public static void RVOMove(this TransformComponent self, TSVector2 forward)
        {self.LSRoom()?.ProcessLog.LogFunction(91, self.LSParent().Id, forward.x.V, forward.y.V);
            if (forward.sqrMagnitude < FP.EN4) {
                self.SetMoving(false);
                return;
            }
            FlagComponent flagComponent = self.LSOwner().GetComponent<FlagComponent>();
            if (flagComponent.HasRestrict(FlagRestrict.NotMove)) {
                self.SetMoving(false);
                return;
            }
            
            self.Forward = new TSVector(forward.x, 0, forward.y);
            self.SetMoving(true);
            
            PropComponent propComponent = self.LSOwner().GetComponent<PropComponent>();
            self.RVO2PrefVelocity = forward.normalized * propComponent.Get(NumericType.Speed);
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.setAgentPrefVelocity(self.LSOwner(), self.RVO2PrefVelocity);
        }

        private static void SetMoving(this TransformComponent self, bool moving)
        {self.LSRoom()?.ProcessLog.LogFunction(90, self.LSParent().Id, moving ? 1 : 0);
            self.IsMovingCurrent = moving;
            if (moving && !self.IsMovingPrevious) {
                self.IsMovingPrevious = true;
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitMoving() { Id = self.LSOwner().Id, IsMoving = true });
            }
            else if (!moving && self.IsMovingPrevious) {
                self.IsMovingPrevious = false;
                self.RVO2PrefVelocity = TSVector2.zero;
                LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                rvo2Component.setAgentPrefVelocity(self.LSOwner(), self.RVO2PrefVelocity);
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitMoving() { Id = self.LSOwner().Id, IsMoving = false });
            }
        }

        public static void SetPosition(this TransformComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(89, self.LSParent().Id, position.x.V, position.y.V, position.z.V);
            self.Position = position;
        }
        
        public static TSVector GetAttachPoint(this TransformComponent self, AttachPoint attachPoint)
        {
            return self.Position + self.Rotation * new TSVector(0, FP.Ratio(15, 10), 0);
        }
        
        public static TSVector TransformDirection(this TransformComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(88, self.LSParent().Id, position.x.V, position.y.V, position.z.V);
            return self.Rotation * position;
        }
        
        public static TSVector TransformVector(this TransformComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(87, self.LSParent().Id, position.x.V, position.y.V, position.z.V);
            return self.Rotation * position;
        }
        
        public static TSVector TransformPoint(this TransformComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(86, self.LSParent().Id, position.x.V, position.y.V, position.z.V);
            return self.Position + self.Rotation * position;
        }
        
        public static void LookAt(this TransformComponent self, LSUnit lsTarget)
        {self.LSRoom()?.ProcessLog.LogFunction(85, self.LSParent().Id, lsTarget.Id);
            var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
            if (flagComponent.HasRestrict(FlagRestrict.NotRotate)) {
                return;
            }
            
            TSVector position = lsTarget.GetComponent<TransformComponent>().Position;
            position.y = self.Position.y;
            TSVector dir = position - self.Position;
            if (dir.sqrMagnitude > FP.EN4) {
                self.Forward = dir;
            }
        }
    }
}