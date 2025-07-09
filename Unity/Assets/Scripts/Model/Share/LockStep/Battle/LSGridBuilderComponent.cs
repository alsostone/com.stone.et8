using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class LSGridBuilderComponent: LSEntity, IAwake, ISerializeToEntity
    {
        public long PlacementTargetId;
        public int PlacementRotation;
        public EUnitType PlacementType;
        public int PlacementTableId;
        public int PlacementLevel;
        public TSVector2 PlacementDragOffset;
    }
}