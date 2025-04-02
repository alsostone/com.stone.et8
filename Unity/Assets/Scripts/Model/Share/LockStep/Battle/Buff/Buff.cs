using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(BuffComponent))]
    [MemoryPackable]
    public partial class Buff : LSEntity, IAwake<int>, IDestroy, ILSUpdate, ISerializeToEntity
    {
        [BsonIgnore]
        [MemoryPackIgnore]
        public LSUnit Owner => this.GetParent<BuffComponent>().Owner;

        public int BuffId;
        public long StartTime;
        public long IntervalTime;
        public uint LayerCount;
        
        public long Caster;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbSkillBuffRow TbBuffRow => this.tbBuffRow ??= TbSkillBuff.Instance.Get(BuffId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbSkillBuffRow tbBuffRow;
    }
}