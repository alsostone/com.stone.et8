using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TeamComponent : LSEntity, IAwake<TeamType, long>, ISerializeToEntity
    {
        public TeamType Type { get; set; }
        public long OwnerId { get; set; }
    }
}