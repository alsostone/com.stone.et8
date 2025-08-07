using MemoryPack;
using ST.GridBuilder;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BirthComponent : LSEntity, IAwake<int>, ILSUpdate, ISerializeToEntity
    {
        public int BirthFrame;
        public int DurationFrame;
    }
}