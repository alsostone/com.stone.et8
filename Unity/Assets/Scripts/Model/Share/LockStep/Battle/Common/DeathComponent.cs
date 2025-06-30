using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class DeathComponent : LSEntity, IAwake<bool>, IDestroy, ILSUpdate, ISerializeToEntity
    {
        public bool IsDeadRelease;
    }
}