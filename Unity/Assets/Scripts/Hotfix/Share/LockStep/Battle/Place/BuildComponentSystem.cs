namespace ET
{
    [LSEntitySystemOf(typeof(BuildComponent))]
    [EntitySystemOf(typeof(BuildComponent))]
    [FriendOf(typeof(BuildComponent))]
    public static partial class BuildComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuildComponent self, int durationFrame)
        {self.LSRoom()?.ProcessLog.LogFunction(97, self.LSParent().Id, durationFrame);
            self.BuildFrame = self.LSWorld().Frame;
            self.DurationFrame = durationFrame;
            self.LSOwner().GetComponent<FlagComponent>().AddRestrict((int)FlagRestrict.NotProduct);
        }

        [LSEntitySystem]
        private static void LSUpdate(this BuildComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(96, self.LSParent().Id);
            if (self.BuildFrame + self.DurationFrame > self.LSWorld().Frame) {
                self.LSOwner().GetComponent<FlagComponent>().RemoveRestrict((int)FlagRestrict.NotProduct);
                self.Dispose();
            }
        }
        
    }
}