
namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewPlayerComponent))]
    [FriendOf(typeof(LSViewPlayerComponent))]
    public static partial class LSViewPlayerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewPlayerComponent self, long bindCampId, long bindHeroId)
        {
            self.BindCampId = bindCampId;
            self.BindHeroId = bindHeroId;
        }
    }
}