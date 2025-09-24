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
        public long PlacementItemId;
        public long PlacementItemTargetId;
        public TSVector2 PlacementDragOffset;
    }
}