using System;
using MemoryPack;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using NPBehave;

namespace ET
{
    [ComponentOf(typeof(LSWorld))]
    [MemoryPackable]
    public partial class AIWorldComponent : LSEntity, IAwake, IDestroy, ILSUpdate, IDeserialize, ISerializeToEntity
    {
        public NPBehave.BehaveWorld BehaveWorld;
        public List<long> NeedStartUnits { get; set; }
        
        [MemoryPackIgnore, BsonIgnore]
        public Dictionary<string, Func<Node>> NodeFactory;
    }
}