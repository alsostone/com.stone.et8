using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSWorld))]
    [MemoryPackable]
    public partial class LSGridBuilderComponent: LSEntity, IAwake, ISerializeToEntity
    {
        public long PlacementTargetId;
        public int PlacementRotation;
        public EUnitType PlacementType;
        public int PlacementTableId;
        public TSVector2 PlacementDragOffset;
    }
}