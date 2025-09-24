namespace ET
{
    [EntitySystemOf(typeof(ItemComponent))]
    [FriendOf(typeof(ItemComponent))]
    public static partial class ItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ItemComponent self, int tableId)
        {self.LSRoom()?.ProcessLog.LogFunction(65, self.LSParent().Id, tableId);
            self.TableId = tableId;
        }
        
    }
}