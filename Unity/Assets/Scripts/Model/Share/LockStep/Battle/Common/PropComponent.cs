using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    public struct LSPropChange
    {
        public LSUnit Unit;
        public NumericType NumericType;
        public long Old;
        public long New;
    }

    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PropComponent: LSEntity, IAwake, ITransfer
    {
        [BsonIgnore]
        [MemoryPackIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<NumericType, long> NumericDic = new Dictionary<NumericType, long>();
    }
}