namespace ET
{
    [EntitySystemOf(typeof(TypeComponent))]
    [FriendOf(typeof(TypeComponent))]
    public static partial class TypeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TypeComponent self, EUnitType type)
        {
            self.Type = type;
        }
        
    }
}