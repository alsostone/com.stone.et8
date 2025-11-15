namespace ET
{
    [EntitySystemOf(typeof(PlayerComponent))]
    [FriendOf(typeof(PlayerComponent))]
    public static partial class PlayerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PlayerComponent self, long bindId)
        {self.LSRoom()?.ProcessLog.LogFunction(145, self.LSParent().Id);
            self.BindEntityId = bindId;
        }
    }
}