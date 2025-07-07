using MemoryPack;
using ST.GridBuilder;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PlacementComponent : LSEntity, IAwake<PlacedLayer, PlacedLayer, bool[]>, IDeserialize, ISerializeToEntity
    {
        public PlacementData PlacementData { get; set; }
    }
}