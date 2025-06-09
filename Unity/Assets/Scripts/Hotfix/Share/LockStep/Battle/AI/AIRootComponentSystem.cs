using NPBehave;

namespace ET
{
    [EntitySystemOf(typeof(AIRootComponent))]
    [FriendOf(typeof(AIRootComponent))]
    [FriendOf(typeof(AIWorldComponent))]
    public static partial class AIRootComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AIRootComponent self, EUnitType type)
        {
            self.Type = type;
            var worldComponent = self.LSWorld().GetComponent<AIWorldComponent>();
            self.AIRoot = new Root(worldComponent.BehaveWorld, new Sequence());
            self.AIRoot.Start();
        }

        [EntitySystem]
        private static void Destroy(this AIRootComponent self)
        {
            self.AIRoot.Stop();
            self.AIRoot.Dispose();
            self.AIRoot = null;
        }

        [EntitySystem]
        private static void Deserialize(this AIRootComponent self)
        {
            var worldComponent = self.LSWorld().GetComponent<AIWorldComponent>();
            self.AIRoot.SetWorld(worldComponent.BehaveWorld);
        }

    }
}