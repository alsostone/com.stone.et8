using NUnit.Framework;
using NPBehave.Examples;
using MemoryPack;

namespace NPBehave
{
    public class SerializeTest
    {
        [Test]
        public void ShouldEqual_SerializedBehaveWorld()
        {
            var behaveWorld = new BehaveWorld();
            var sharedBlackboard = behaveWorld.GetSharedBlackboard("example-swarm-ai");
            sharedBlackboard.SetBool("sharedBlackboard", true);
            
            var blackboard1 = behaveWorld.CreateBlackboard(sharedBlackboard);
            var blackboard2 = behaveWorld.CreateBlackboard(sharedBlackboard);
            var blackboard3 = behaveWorld.CreateBlackboard(sharedBlackboard);
            var blackboard4 = behaveWorld.CreateBlackboard(sharedBlackboard);

            var blackboard1Guid = blackboard1.Guid;
            var blackboard2Guid = blackboard2.Guid;
            var blackboard3Guid = blackboard3.Guid;
            var blackboard4Guid = blackboard4.Guid;
            
            blackboard1.SetBool("blackboard1", true);
            blackboard2.SetInt("blackboard2", 100);
            blackboard3.SetBool("blackboard3", true);
            blackboard4.SetBool("blackboard4", true);
            
            var bytes = MemoryPackSerializer.Serialize(behaveWorld);
            var behaveWorld1 = MemoryPackSerializer.Deserialize<BehaveWorld>(bytes);
            var sharedBlackboardNew = behaveWorld.GetSharedBlackboard("example-swarm-ai");

            var blackboard1New = behaveWorld1.GetBlackboard(blackboard1Guid);
            var blackboard2New = behaveWorld1.GetBlackboard(blackboard2Guid);
            var blackboard3New = behaveWorld1.GetBlackboard(blackboard3Guid);
            var blackboard4New = behaveWorld1.GetBlackboard(blackboard4Guid);
            
            Assert.AreEqual(blackboard1.GetBool("blackboard1"), blackboard1New.GetBool("blackboard1"));
            Assert.AreEqual(blackboard2.GetInt("blackboard2"), blackboard2New.GetInt("blackboard2"));
            Assert.AreEqual(blackboard3.GetBool("blackboard3"), blackboard3New.GetBool("blackboard3"));
            Assert.AreEqual(blackboard4.GetBool("blackboard4"), blackboard4New.GetBool("blackboard4"));
            
            Assert.AreEqual(sharedBlackboard.GetBool("sharedBlackboard"), sharedBlackboardNew.GetBool("sharedBlackboard"));
            Assert.AreEqual(behaveWorld.Clock.NumTimers, 5);
            Assert.AreEqual(behaveWorld1.Clock.NumTimers, 5);
            
            behaveWorld.Update(1f);
            behaveWorld1.Update(1f);
            
            Assert.AreEqual(behaveWorld.Clock.NumTimers, 0);
            Assert.AreEqual(behaveWorld1.Clock.NumTimers, 0);
        }
        
        [Test]
        public void ShouldEqual_SerializedBehaveWorldAndRoot()
        {
            var world = new BehaveWorld();
            var root1 = new Root(world, new Sequence(new ActionLog("root1 1"), new ActionLog("root1 2")));
            var root2 = new Root(world, new Sequence(new ActionLog("root2 1"), new ActionLog("root2 2")));
            
            var worldBytes = MemoryPackSerializer.Serialize(world);
            var root1Bytes = MemoryPackSerializer.Serialize(root1);
            var root2Bytes = MemoryPackSerializer.Serialize(root2);
            
            world = MemoryPackSerializer.Deserialize<BehaveWorld>(worldBytes);
            
            // 反序列化节点1并重建其上下文
            root1 = MemoryPackSerializer.Deserialize<Root>(root1Bytes);
            root1.SetWorld(world);
            
            // 反序列化节点2并重建其上下文
            root2 = MemoryPackSerializer.Deserialize<Root>(root2Bytes);
            root2.SetWorld(world);
        }

    }
}