using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BuffComponent : LSEntity, IAwake, IDestroy, ISerializeToEntity
    {
        [BsonIgnore]
        [MemoryPackIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public SortedDictionary<int, long> IdBuffMap = new();
    }
}