using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class BuffComponent : LSEntity, IAwake, IDestroy
    {
        [BsonIgnore]
        public Unit Owner => this.GetParent<Unit>();
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public SortedDictionary<int, EntityRef<Buff>> IdBuffMap = new();
    }
}