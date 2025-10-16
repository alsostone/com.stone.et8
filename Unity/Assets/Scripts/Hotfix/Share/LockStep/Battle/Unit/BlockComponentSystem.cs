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
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddStaticAgent(self.LSOwner());
        }
        
        [EntitySystem]
        private static void Destroy(this BlockComponent self)
        {
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.RemoveAgent(self.LSOwner());
        }
        
        [EntitySystem]
        private static void Deserialize(this BlockComponent self)
        {
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddStaticAgent(self.LSOwner());
        }
    }
}