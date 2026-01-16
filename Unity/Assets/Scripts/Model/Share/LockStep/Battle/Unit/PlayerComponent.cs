using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PlayerComponent : LSEntity, IAwake<long, long>, ISerializeToEntity
    {
        public long BindCampId { get; set; }
        public long BindHeroId { get; set; }
    }
}