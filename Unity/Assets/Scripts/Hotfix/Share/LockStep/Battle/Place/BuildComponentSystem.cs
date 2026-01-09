using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(BuildComponent))]
    [EntitySystemOf(typeof(BuildComponent))]
    [FriendOf(typeof(BuildComponent))]
    public static partial class BuildComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuildComponent self, int duration)
        {self.LSRoom()?.ProcessLog.LogFunction(97, self.LSParent().Id, duration);
            self.StartTime = self.LSWorld().ElapsedTime;
            self.DurationTime = duration * FP.EN3;
            self.LSOwner().GetComponent<FlagComponent>().AddRestrict((int)FlagRestrict.NotProduct);
        }

        [LSEntitySystem]
        private static void LSUpdate(this BuildComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(96, self.LSParent().Id);
            if (self.StartTime + self.DurationTime > self.LSWorld().ElapsedTime) {
                self.LSOwner().GetComponent<FlagComponent>().RemoveRestrict((int)FlagRestrict.NotProduct);
                self.Dispose();
            }
        }
        
    }
}