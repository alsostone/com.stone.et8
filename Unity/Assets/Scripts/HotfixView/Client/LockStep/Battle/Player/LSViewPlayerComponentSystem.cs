
namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewPlayerComponent))]
    [FriendOf(typeof(LSViewPlayerComponent))]
    public static partial class LSViewPlayerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewPlayerComponent self, long bindViewId)
        {
            self.BindHeroId = bindViewId;
        }
    }
}