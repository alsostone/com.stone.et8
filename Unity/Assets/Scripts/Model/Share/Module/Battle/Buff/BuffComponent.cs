using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class BuffComponent : Entity, IAwake, IDestroy
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public SortedDictionary<int, EntityRef<Buff>> IdBuffMapping = new();
    }
}