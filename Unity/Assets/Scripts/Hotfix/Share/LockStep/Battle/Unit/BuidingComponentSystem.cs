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
        }
        
    }
}