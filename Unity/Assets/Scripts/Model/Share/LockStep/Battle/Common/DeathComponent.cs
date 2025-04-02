using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class DeathComponent : LSEntity, IAwake<bool>, IDestroy, ISerializeToEntity
    {
        [BsonIgnore]
        [MemoryPackIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();

        public bool Dead;
        public bool IsDeadRelease;
    }
}