using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public struct PropChange
    {
        public LSUnit Unit;
        public NumericType NumericType;
        public long Old;
        public long New;
    }

    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PropComponent: LSEntity, IAwake, ISerializeToEntity
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<NumericType, long> NumericDic = new Dictionary<NumericType, long>();
    }
}