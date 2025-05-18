using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using TrueSync;

namespace ET
{
    public struct PropChange
    {
        public long Id;
        public NumericType NumericType;
        public FP Old;
        public FP New;
    }

    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PropComponent: LSEntity, IAwake, ISerializeToEntity
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<NumericType, FP> NumericDic = new Dictionary<NumericType, FP>();
    }
}