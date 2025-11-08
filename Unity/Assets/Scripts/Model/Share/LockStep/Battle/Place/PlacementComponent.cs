using MemoryPack;
using ST.GridBuilder;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class PlacementComponent : LSEntity, IAwake<PlacementData>, IDestroy, IDeserialize, ISerializeToEntity
    {
        public PlacementData PlacementData;
    }
}