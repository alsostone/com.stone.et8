using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class BeHitComponent : LSEntity, IAwake, IDestroy
    {
        [BsonIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();
        
        public List<EntityRef<LSUnit>> Attackers = new();
    }
}