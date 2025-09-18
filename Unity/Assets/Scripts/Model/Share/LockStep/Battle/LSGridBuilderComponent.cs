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
        public int PlacementIndex = -1;
        public TSVector2 PlacementDragOffset;
    }
}