using MemoryPack;
using ST.GridBuilder;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PlacementComponent : LSEntity, IAwake<PlacementData>, IDeserialize, ISerializeToEntity
    {
        public PlacementData PlacementData { get; set; }
    }
}