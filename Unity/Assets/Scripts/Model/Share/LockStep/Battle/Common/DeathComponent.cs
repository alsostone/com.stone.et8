using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class DeathComponent : LSEntity, IAwake<bool>, IDestroy
    {
        [BsonIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();

        public bool Dead;
        public bool IsDeadRelease;
    }
}