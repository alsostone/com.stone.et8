namespace ET
{
    [LSEntitySystemOf(typeof(BirthComponent))]
    [EntitySystemOf(typeof(BirthComponent))]
    [FriendOf(typeof(BirthComponent))]
    public static partial class BirthComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BirthComponent self, int durationFrame)
        {
            self.LSOwner().EnabledRef -= 1;
            self.BirthFrame = self.LSWorld().Frame;
            self.DurationFrame = durationFrame;
        }

        [LSEntitySystem]
        private static void LSUpdate(this BirthComponent self)
        {
            if (self.BirthFrame + self.DurationFrame > self.LSWorld().Frame)
            {
                self.LSOwner().EnabledRef += 1;
                self.Dispose();
            }
        }
        
    }
}