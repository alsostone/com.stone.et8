using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(AIWorldComponent))]
    [EntitySystemOf(typeof(AIWorldComponent))]
    [FriendOf(typeof(AIWorldComponent))]
    public static partial class AIWorldComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AIWorldComponent self)
        {
            self.BehaveWorld = new NPBehave.BehaveWorld();
            self.BehaveWorld.SetRandom(self.GetRandom());
        }
        
        [EntitySystem]
        private static void Destroy(this AIWorldComponent self)
        {
            self.BehaveWorld.Dispose();
            self.BehaveWorld = null;
        }
        
        [EntitySystem]
        private static void Deserialize(this AIWorldComponent self)
        {
            self.BehaveWorld.SetRandom(self.GetRandom());
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this AIWorldComponent self)
        {
            self.BehaveWorld.Update(LSConstValue.UpdateInterval * FP.EN3);
        }
        
    }
}