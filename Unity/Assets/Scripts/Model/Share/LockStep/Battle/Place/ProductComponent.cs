using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class ProductComponent : LSEntity, IAwake<int>, ILSUpdate, ISerializeToEntity
    {
        public int ProductSkillId;
        public FP StartTime;
        public FP IntervalTime;
    }
}