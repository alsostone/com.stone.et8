using MemoryPack;
using ST.GridBuilder;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BuildComponent : LSEntity, IAwake<int>, ILSUpdate, ISerializeToEntity
    {
        public FP StartTime;
        public FP DurationTime;
    }
}