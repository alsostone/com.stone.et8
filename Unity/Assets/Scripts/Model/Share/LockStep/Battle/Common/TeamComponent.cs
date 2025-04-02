using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TeamComponent : LSEntity, IAwake<TeamType>, ISerializeToEntity
    {
        public TeamType Type;
    }
}