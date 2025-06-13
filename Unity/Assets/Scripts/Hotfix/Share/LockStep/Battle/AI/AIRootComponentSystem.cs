using NPBehave;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(AIRootComponent))]
    [FriendOf(typeof(AIRootComponent))]
    [FriendOf(typeof(AIWorldComponent))]
    public static partial class AIRootComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AIRootComponent self, Node node)
        {self.LSRoom()?.ProcessLog.LogFunction(5, self.LSParent().Id);
            var worldComponent = self.LSWorld().GetComponent<AIWorldComponent>();
            self.AIRoot = new Root(worldComponent.BehaveWorld, node, self.LSOwner());
            self.AIRoot.Start();
        }

        [EntitySystem]
        private static void Destroy(this AIRootComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(4, self.LSParent().Id);
            self.AIRoot.Stop();
            self.AIRoot.Dispose();
            self.AIRoot = null;
        }

        [EntitySystem]
        private static void Deserialize(this AIRootComponent self)
        {
            var worldComponent = self.LSWorld().GetComponent<AIWorldComponent>();
            self.AIRoot.SetWorld(worldComponent.BehaveWorld, self.LSOwner());
        }

    }
}