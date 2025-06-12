namespace ET
{
    [EntitySystemOf(typeof(BuildingComponent))]
    [FriendOf(typeof(BuildingComponent))]
    public static partial class BuildingComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuildingComponent self, int tableId, int level)
        {self.LSRoom()?.ProcessLog.LogFunction(64, self.LSParent().Id, tableId, level);
            self.TableId = tableId;
            self.Level = level;
        }
        
    }
}