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
        {self.LSRoom()?.ProcessLog.LogFunction(43, self.LSParent().Id);
            self.Position = position;
            self.Rotation = rotation;
        }

        [LSEntitySystem]
        private static void LSUpdate(this TransformComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(99, self.LSParent().Id);
            if (self.IsMovingCurrent) {
                self.IsMovingCurrent = false;
            }
            else if (self.IsMovingPrevious) {
                self.SetMoving(false);
            }
        }
        
        public static void Move(this TransformComponent self, TSVector2 forward)
        {self.LSRoom()?.ProcessLog.LogFunction(42, self.LSParent().Id);
            if (forward.sqrMagnitude < FP.EN4) {
                self.SetMoving(false);
                return;
            }
            FlagComponent flagComponent = self.LSOwner().GetComponent<FlagComponent>();
            if (flagComponent.HasRestrict(FlagRestrict.NotMove)) {
                self.SetMoving(false);
                return;
            }
            
            self.SetMoving(true);
            PropComponent propComponent = self.LSOwner().GetComponent<PropComponent>();
            TSVector2 v2 = forward.normalized * propComponent.Get(NumericType.Speed) * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            if (v2.LengthSquared() > FP.EN4)
            {
                TSVector position = self.Position;
                self.Position += new TSVector(v2.x, 0, v2.y);
                self.Forward = self.Position - position;
                LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                rvo2Component.SetAgentPosition(self.LSOwner(), self.Position);
            }
        }
        
        public static void RVOMove(this TransformComponent self, TSVector2 forward)
        {
            if (forward.sqrMagnitude < FP.EN4) {
                self.SetMoving(false);
                return;
            }
            FlagComponent flagComponent = self.LSOwner().GetComponent<FlagComponent>();
            if (flagComponent.HasRestrict(FlagRestrict.NotMove)) {
                self.SetMoving(false);
                return;
            }
            
            self.SetMoving(true);
            PropComponent propComponent = self.LSOwner().GetComponent<PropComponent>();
            TSVector2 v2 = forward.normalized * propComponent.Get(NumericType.Speed);
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.setAgentPrefVelocity(self.LSOwner(), v2);
        }

        private static void SetMoving(this TransformComponent self, bool moving)
        {self.LSRoom()?.ProcessLog.LogFunction(70, self.LSParent().Id, moving ? 1 : 0);
            self.IsMovingCurrent = moving;
            if (moving && !self.IsMovingPrevious) {
                self.IsMovingPrevious = true;
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitMoving() { Id = self.LSOwner().Id, IsMoving = true });
            }
            else if (!moving && self.IsMovingPrevious) {
                self.IsMovingPrevious = false;
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitMoving() { Id = self.LSOwner().Id, IsMoving = false });
            }
        }

        public static void SetPosition(this TransformComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(85, self.LSParent().Id);
            self.Position = position;
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.SetAgentPosition(self.LSOwner(), position);
        }
        
        public static TSVector TransformDirection(this TransformComponent self, TSVector position)
        {
            self.LSRoom()?.ProcessLog.LogIgnore();
            return self.Rotation * position;
        }
        
        public static TSVector TransformVector(this TransformComponent self, TSVector position)
        {
            self.LSRoom()?.ProcessLog.LogIgnore();
            return self.Rotation * position;
        }
        
        public static TSVector TransformPoint(this TransformComponent self, TSVector position)
        {
            self.LSRoom()?.ProcessLog.LogIgnore();
            return self.Position + self.Rotation * position;
        }
    }
}