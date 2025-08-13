using MemoryPack;
using ST.GridBuilder;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BuildComponent : LSEntity, IAwake<int>, ILSUpdate, ISerializeToEntity
    {
        public int BuildFrame;
        public int DurationFrame;
    }
}