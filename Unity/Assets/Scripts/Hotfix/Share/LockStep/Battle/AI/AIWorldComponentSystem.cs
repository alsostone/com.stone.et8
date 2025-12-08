using System;
using System.Collections.Generic;
using NPBehave;
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
        {self.LSRoom()?.ProcessLog.LogFunction(32, self.LSParent().Id);
            self.BehaveWorld = new NPBehave.BehaveWorld();
            self.BehaveWorld.SetRandom(self.GetRandom());
            self.NeedStartUnits = new List<long>();
            self.GenAINodeFactory();
        }
        
        [EntitySystem]
        private static void Destroy(this AIWorldComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(31, self.LSParent().Id);
            self.BehaveWorld.Dispose();
            self.BehaveWorld = null;
            self.NeedStartUnits.Clear();
        }
        
        [EntitySystem]
        private static void Deserialize(this AIWorldComponent self)
        {
            self.BehaveWorld.SetRandom(self.GetRandom());
            self.GenAINodeFactory();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this AIWorldComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(30, self.LSParent().Id);
            self.BehaveWorld.Update(LSConstValue.UpdateInterval * FP.EN3);
            
            for (int i = self.NeedStartUnits.Count - 1; i >= 0; i--)
            {
                LSUnit lsUnit = self.LSUnit(self.NeedStartUnits[i]);
                lsUnit.GetComponent<AIRootComponent>().Start();
                self.NeedStartUnits.RemoveAt(i);
            }
        }
        
        private static void GenAINodeFactory(this AIWorldComponent self)
        {
            self.NodeFactory = new Dictionary<string, Func<Node>>();
            self.NodeFactory.Add("AIAutoAttack", AIAutoAttack.Gen);
            self.NodeFactory.Add("AIAutoAttackCenter", AIAutoAttackCenter.Gen);
        }
        
        public static Node GenAINode(this AIWorldComponent self, string aiName)
        {
            if (self.NodeFactory.TryGetValue(aiName, out Func<Node> func))
            {
                return func();
            }
            return null;
        }
    }
}