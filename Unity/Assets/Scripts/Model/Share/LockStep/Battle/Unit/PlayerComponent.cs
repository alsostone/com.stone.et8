using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PlayerComponent : LSEntity, IAwake, ISerializeToEntity
    {
        public long BindCampId { get; set; }
        public long BindHeroId { get; set; }
    }
}