namespace ET
{
    [EntitySystemOf(typeof(HeroComponent))]
    [FriendOf(typeof(HeroComponent))]
    public static partial class HeroComponentSystem
    {
        [EntitySystem]
        private static void Awake(this HeroComponent self, int tableId)
        {self.LSRoom()?.ProcessLog.LogFunction(66, self.LSParent().Id, tableId);
            self.TableId = tableId;
        }
        
    }
}