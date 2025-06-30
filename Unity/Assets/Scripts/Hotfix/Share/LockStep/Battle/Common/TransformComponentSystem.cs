using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(TransformComponent))]
    [FriendOf(typeof(TransformComponent))]
    public static partial class TransformComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TransformComponent self, TSVector position, TSQuaternion rotation)
        {self.LSRoom()?.ProcessLog.LogFunction(43, self.LSParent().Id);
            self.Position = position;
            self.Rotation = rotation;
            self.IsMoving = false;
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
            TSVector2 v2 = forward * propComponent.Get(NumericType.Speed) * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            if (v2.LengthSquared() > FP.EN4)
            {
                TSVector position = self.Position;
                self.Position += new TSVector(v2.x, 0, v2.y);
                self.Forward = self.Position - position;
            }
        }

        private static void SetMoving(this TransformComponent self, bool moving)
        {self.LSRoom()?.ProcessLog.LogFunction(70, self.LSParent().Id, moving ? 1 : 0);
            if (moving == self.IsMoving) return;
            self.IsMoving = moving;
            EventSystem.Instance.Publish(self.LSWorld(), new LSUnitMoving() { Id = self.LSOwner().Id, IsMoving = self.IsMoving });
        }
    }
}