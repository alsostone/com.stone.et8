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
        {self.LSRoom()?.ProcessLog.LogFunction(29, self.LSParent().Id);
            var worldComponent = self.LSWorld().GetComponent<AIWorldComponent>();
            self.AIRoot = new Root(worldComponent.BehaveWorld, node, self.LSOwner());
            
            // 不能在创建时调用Start，因为上下文可能还未完全初始化 如：AI的节点中用到TransformComponent组件但他还未被添加到实体
            AIWorldComponent aiWorldComponent = self.LSWorld().GetComponent<AIWorldComponent>();
            aiWorldComponent.NeedStartUnits.Add(self.LSOwner().Id);
        }

        public static void Start(this AIRootComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(28, self.LSParent().Id);
            self.AIRoot.Start();
        }

        [EntitySystem]
        private static void Destroy(this AIRootComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(27, self.LSParent().Id);
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