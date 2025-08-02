using System;
using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable]
    public partial class BehaveWorld : IDisposable
    {
        [BsonElement][MemoryPackInclude] public Clock Clock { get; private set; }
        
        [BsonElement][MemoryPackInclude] private int currentGuid = 0;
        [BsonElement][MemoryPackInclude] private List<Blackboard> blackboards;
        [BsonElement][MemoryPackInclude] private Dictionary<string, int> sharedBlackboards;
        
        [BsonIgnore][MemoryPackIgnore] public Dictionary<long, Receiver> GuidReceiverMapping = new Dictionary<long, Receiver>();
        [BsonIgnore][MemoryPackIgnore] private TSRandom random;
        
        public BehaveWorld()
        {
            Clock = new Clock(this);
            blackboards = new List<Blackboard>();
            sharedBlackboards = new Dictionary<string, int>();
        }
        
        [MemoryPackConstructor]
        private BehaveWorld(Clock clock, List<Blackboard> blackboards, Dictionary<string, int> sharedBlackboards)
        {
            Clock = clock;
            this.blackboards = blackboards;
            this.sharedBlackboards = sharedBlackboards;
        }
        
        [MemoryPackOnDeserialized]
        public void OnDeserialized() 
        {
            Clock.SetWorld(this);
            foreach (var blackboard in blackboards)
            {
                blackboard.SetWorld(this);
            }
        }
        
        // 防止因为循环依赖导致无法GC
        public void Dispose()
        {
            Clock.Dispose();
            Clock = null;
            foreach (var blackboard in blackboards)
            {
                blackboard.Dispose();
            }
            blackboards.Clear();
            sharedBlackboards.Clear();
            GuidReceiverMapping.Clear();
            random = null;
        }

        public void SetRandom(TSRandom tsRandom)
        {
            this.random = tsRandom;
        }
        
        public int GetNextGuid()
        {
            return ++currentGuid;
        }
        
        public Blackboard GetBlackboard(long guid)
        {
            if (GuidReceiverMapping.TryGetValue(guid, out var receiver))
            {
                return receiver as Blackboard;
            }
            return null;
        }

        public Blackboard CreateBlackboard(Blackboard parent = null)
        {
            var blackboard = new Blackboard(this, parent);
            blackboards.Add(blackboard);
            return blackboard;
        }
        
        public Blackboard GetSharedBlackboard(string key)
        {
            Blackboard blackboard;
            if (sharedBlackboards.TryGetValue(key, out var guid))
            {
                blackboard = GetBlackboard(guid);
            }
            else
            {
                blackboard = CreateBlackboard();
                sharedBlackboards.Add(key, blackboard.Guid);
            }
            return blackboard;
        }

        public void Update(FP deltaTime)
        {
            Clock.Update(deltaTime);
        }

        public int GetRandomNext(int range)
        {
            if (random == null)
                random = new TSRandom(0);
            return this.random.Next(0, range);
        }

    }
}