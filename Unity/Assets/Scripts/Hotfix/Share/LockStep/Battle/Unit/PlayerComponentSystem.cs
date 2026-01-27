namespace ET
{
    [EntitySystemOf(typeof(PlayerComponent))]
    [FriendOf(typeof(PlayerComponent))]
    public static partial class PlayerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PlayerComponent self)
        {
        }
        
        public static void SetBindEntities(this PlayerComponent self, long campId, long heroId)
        {
            self.BindCampId = campId;
            self.BindHeroId = heroId;
        }
    }
}