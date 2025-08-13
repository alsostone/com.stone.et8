using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class WorkComponent : LSEntity, IAwake<int>, IDeserialize, ISerializeToEntity
    {
        public int WorkerCount { get; set; }
        public int WorkerLimit { get; set; }
    }
}