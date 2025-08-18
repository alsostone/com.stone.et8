namespace ET
{
    [EntitySystemOf(typeof(BuildingComponent))]
    [FriendOf(typeof(BuildingComponent))]
    public static partial class BuildingComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuildingComponent self, int tableId)
        {
            self.TableId = tableId;
        }
        
    }
}