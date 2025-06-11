namespace ET
{
    [EntitySystemOf(typeof(SoldierComponent))]
    [FriendOf(typeof(SoldierComponent))]
    public static partial class SoldierComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SoldierComponent self, int tableId, int level)
        {self.LSRoom().ProcessLog.LogFunction(67, self.LSParent().Id, tableId, level);
            self.TableId = tableId;
            self.Level = level;
        }
        
    }
}