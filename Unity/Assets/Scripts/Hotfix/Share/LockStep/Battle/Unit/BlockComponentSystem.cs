namespace ET
{
    [EntitySystemOf(typeof(BlockComponent))]
    [FriendOf(typeof(BlockComponent))]
    public static partial class BlockComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BlockComponent self, int tableId)
        {self.LSRoom()?.ProcessLog.LogFunction(88, self.LSParent().Id, tableId);
            self.TableId = tableId;
        }
        
    }
}