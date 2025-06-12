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
        }

        public static void Move(this TransformComponent self, TSVector2 forward)
        {self.LSRoom()?.ProcessLog.LogFunction(42, self.LSParent().Id);
            FlagComponent flagComponent = self.LSOwner().GetComponent<FlagComponent>();
            if (flagComponent.HasRestrict(FlagRestrict.NotMove)) { return; }
            
            PropComponent propComponent = self.LSOwner().GetComponent<PropComponent>();
            TSVector2 v2 = forward * propComponent.Get(NumericType.Speed) * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            if (v2.LengthSquared() > FP.EN4)
            {
                TSVector position = self.Position;
                self.Position += new TSVector(v2.x, 0, v2.y);
                self.Forward = self.Position - position;
            }
        }
        
    }
}