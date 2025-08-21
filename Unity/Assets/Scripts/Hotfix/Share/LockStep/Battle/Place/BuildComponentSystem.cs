namespace ET
{
    [LSEntitySystemOf(typeof(BuildComponent))]
    [EntitySystemOf(typeof(BuildComponent))]
    [FriendOf(typeof(BuildComponent))]
    public static partial class BuildComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuildComponent self, int durationFrame)
        {self.LSRoom()?.ProcessLog.LogFunction(101, self.LSParent().Id, durationFrame);
            self.LSOwner().EnabledRef -= 1;
            self.BuildFrame = self.LSWorld().Frame;
            self.DurationFrame = durationFrame;
        }

        [LSEntitySystem]
        private static void LSUpdate(this BuildComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(100, self.LSParent().Id);
            if (self.BuildFrame + self.DurationFrame > self.LSWorld().Frame)
            {
                self.LSOwner().EnabledRef += 1;
                self.Dispose();
            }
        }
        
    }
}