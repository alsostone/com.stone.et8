namespace ET
{
    [EntitySystemOf(typeof(BlockComponent))]
    [FriendOf(typeof(BlockComponent))]
    public static partial class BlockComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BlockComponent self, int tableId)
        {
            self.TableId = tableId;
        }
        
    }
}