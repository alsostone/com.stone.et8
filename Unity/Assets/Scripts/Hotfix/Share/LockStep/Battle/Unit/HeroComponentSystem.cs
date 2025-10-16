namespace ET
{
    [EntitySystemOf(typeof(HeroComponent))]
    [FriendOf(typeof(HeroComponent))]
    public static partial class HeroComponentSystem
    {
        [EntitySystem]
        private static void Awake(this HeroComponent self, int tableId, int skinId)
        {
            self.TableId = tableId;
            self.SkinId = skinId;
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddStaticAgent(self.LSOwner());
        }
        
        [EntitySystem]
        private static void Destroy(this HeroComponent self)
        {
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.RemoveAgent(self.LSOwner());
        }
        
        [EntitySystem]
        private static void Deserialize(this HeroComponent self)
        {
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddStaticAgent(self.LSOwner());
        }
    }
}