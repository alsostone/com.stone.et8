using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PlayerComponent : LSEntity, IAwake<long>, ISerializeToEntity
    {
        public long BindEntityId { get; set; }
    }
}