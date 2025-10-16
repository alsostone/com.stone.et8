namespace ET
{
    [EntitySystemOf(typeof(BuildingComponent))]
    [FriendOf(typeof(BuildingComponent))]
    public static partial class BuildingComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuildingComponent self, int tableId)
        {self.LSRoom()?.ProcessLog.LogFunction(107, self.LSParent().Id, tableId);
            self.TableId = tableId;
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddStaticAgent(self.LSOwner());
        }
        
        [EntitySystem]
        private static void Destroy(this BuildingComponent self)
        {
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.RemoveAgent(self.LSOwner());
        }
        
        [EntitySystem]
        private static void Deserialize(this BuildingComponent self)
        {
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddStaticAgent(self.LSOwner());
        }
    }
}