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
        }
        
    }
}