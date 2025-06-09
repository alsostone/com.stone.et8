using ET;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using TrueSync;
using UnityEngine;

namespace NPBehave
{
    public static class MongoSerialized
    {
        public static void ShouldEqual_MongoDBSerializedBehaveWorldAndRoot()
        {
            var world = new BehaveWorld();
            var root1 = new Root(world, new Sequence(new IncrBlackboardKey("currentActionRoot1")));
            var root2 = new Root(world, new Sequence(new IncrBlackboardKey("currentActionRoot2")));
            root1.Start();
            root2.Start();
                
            world.Update(1);
            UnityEngine.Assertions.Assert.AreEqual(root1.RootBlackboard.GetInt("currentActionRoot1"), 1);
            UnityEngine.Assertions.Assert.AreEqual(root2.RootBlackboard.GetInt("currentActionRoot2"), 1);

            BsonSerializer.RegisterSerializer(typeof (FP), new StructBsonSerialize<FP>());
            BsonSerializer.RegisterSerializer(typeof (Notification), new StructBsonSerialize<Notification>());

            root1.RootBlackboard.SetInt("currentActionRoot1", 2);
            root2.RootBlackboard.SetInt("currentActionRoot2", 2);
            
            var worldBytes = world.ToJson();
            var root1Bytes = root1.ToJson();
            var root2Bytes = root2.ToJson();
            
            Debug.Log(worldBytes);
            Debug.Log(root1Bytes);
            Debug.Log(root2Bytes);
            
            var worldNew = BsonSerializer.Deserialize<BehaveWorld>(worldBytes);
            worldNew.OnDeserialized();
            
            // 反序列化节点1并重建其上下文
            var root1New = BsonSerializer.Deserialize<Root>(root1Bytes);
            root1New.SetWorld(worldNew);
            
            // 反序列化节点2并重建其上下文
            var root2New = BsonSerializer.Deserialize<Root>(root2Bytes);
            root2New.SetWorld(worldNew);
            
            world.Update(1);
            worldNew.Update(1);
            
            UnityEngine.Assertions.Assert.AreEqual(root1New.RootBlackboard.GetInt("currentActionRoot1"), 3);
            UnityEngine.Assertions.Assert.AreEqual(root2New.RootBlackboard.GetInt("currentActionRoot2"), 3);
            
            world.Update(1);
            worldNew.Update(1);
            UnityEngine.Assertions.Assert.AreEqual(root1.RootBlackboard.GetInt("currentActionRoot1"), root1New.RootBlackboard.GetInt("currentActionRoot1"));
            UnityEngine.Assertions.Assert.AreEqual(root2.RootBlackboard.GetInt("currentActionRoot2"), root2New.RootBlackboard.GetInt("currentActionRoot2"));
        }
    }
}
