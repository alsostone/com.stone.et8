using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(BuffComponent))]
    [MemoryPackable]
    public partial class Buff : LSEntity, IAwake<int>, IDestroy, ILSUpdate, ISerializeToEntity
    {
        public int BuffId;
        public uint LayerCount;
        
        public long Caster;
        public int StartFrame;
        public int IntervalFrame;
        public int EndFrame;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbSkillBuffRow TbBuffRow => this.tbBuffRow ??= TbSkillBuff.Instance.Get(BuffId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbSkillBuffRow tbBuffRow;
    }
}