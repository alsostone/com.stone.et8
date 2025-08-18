namespace ET
{
    [EntitySystemOf(typeof(SoldierComponent))]
    [FriendOf(typeof(SoldierComponent))]
    public static partial class SoldierComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SoldierComponent self, int tableId)
        {
            self.TableId = tableId;
        }
        
    }
}