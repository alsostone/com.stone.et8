namespace ET
{
    [EntitySystemOf(typeof(SoldierComponent))]
    [FriendOf(typeof(SoldierComponent))]
    public static partial class SoldierComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SoldierComponent self, int tableId)
        {self.LSRoom()?.ProcessLog.LogFunction(108, self.LSParent().Id, tableId);
            self.TableId = tableId;
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddDynamicAgent(self.LSOwner());
        }
        
        [EntitySystem]
        private static void Destroy(this SoldierComponent self)
        {
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.RemoveAgent(self.LSOwner());
        }
        
        [EntitySystem]
        private static void Deserialize(this SoldierComponent self)
        {
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddDynamicAgent(self.LSOwner());
        }
    }
}