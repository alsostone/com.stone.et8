using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class ProductComponent : LSEntity, IAwake<int>, ILSUpdate, ISerializeToEntity
    {
        public int ProductSkillId;
        public int ProductFrame;
        public int IntervalFrame;
    }
}