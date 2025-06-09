using NUnit.Framework;
using MemoryPack;
using TrueSync;

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
            
            behaveWorld.Update(FP.One);
            behaveWorld1.Update(FP.One);
            
            Assert.AreEqual(behaveWorld.Clock.NumTimers, 0);
            Assert.AreEqual(behaveWorld1.Clock.NumTimers, 0);
        }
        
        [Test]
        public void ShouldEqual_SerializedBehaveWorldAndRoot()
        {
            var world = new BehaveWorld();
            var root1 = new Root(world, new Sequence(new IncrBlackboardKey("currentActionRoot1")));
            var root2 = new Root(world, new Sequence(new IncrBlackboardKey("currentActionRoot2")));
            root1.Start();
            root2.Start();
            
            world.Update(1);
            Assert.AreEqual(root1.RootBlackboard.GetInt("currentActionRoot1"), 1);
            Assert.AreEqual(root2.RootBlackboard.GetInt("currentActionRoot2"), 1);
            
            var worldBytes = MemoryPackSerializer.Serialize(world);
            var root1Bytes = MemoryPackSerializer.Serialize(root1);
            var root2Bytes = MemoryPackSerializer.Serialize(root2);
            
            var worldNew = MemoryPackSerializer.Deserialize<BehaveWorld>(worldBytes);
            
            // 反序列化节点1并重建其上下文
            var root1New = MemoryPackSerializer.Deserialize<Root>(root1Bytes);
            root1New.SetWorld(worldNew);
            
            // 反序列化节点2并重建其上下文
            var root2New = MemoryPackSerializer.Deserialize<Root>(root2Bytes);
            root2New.SetWorld(worldNew);
            
            world.Update(1);
            worldNew.Update(1);
            
            Assert.AreEqual(root1New.RootBlackboard.GetInt("currentActionRoot1"), 2);
            Assert.AreEqual(root2New.RootBlackboard.GetInt("currentActionRoot2"), 2);
            
            world.Update(1);
            worldNew.Update(1);
            Assert.AreEqual(root1.RootBlackboard.GetInt("currentActionRoot1"), root1New.RootBlackboard.GetInt("currentActionRoot1"));
            Assert.AreEqual(root2.RootBlackboard.GetInt("currentActionRoot2"), root2New.RootBlackboard.GetInt("currentActionRoot2"));
        }

        [Test]
        public void ShouldEqual_MongoDBSerializedBehaveWorldAndRoot()
        {
            MongoSerialized.ShouldEqual_MongoDBSerializedBehaveWorldAndRoot();
        }

    }
}