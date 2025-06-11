namespace ET
{
    [EntitySystemOf(typeof(DropComponent))]
    [FriendOf(typeof(DropComponent))]
    public static partial class DropComponentSystem
    {
        [EntitySystem]
        private static void Awake(this DropComponent self, int tableId)
        {
            self.TableId = tableId;
        }
        
    }
}