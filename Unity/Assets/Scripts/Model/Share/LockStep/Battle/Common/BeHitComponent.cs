using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BeHitComponent : LSEntity, IAwake, IDestroy, ISerializeToEntity
    {
        [BsonIgnore]
        [MemoryPackIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();
        
        public List<long> Attackers = new();
    }
}