using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(BuffComponent))]
    [MemoryPackable]
    public partial class Buff : LSEntity, IAwake<int, LSUnit>, IDestroy, ILSUpdate, ISerializeToEntity
    {
        public int BuffId;
        public int LayerCount;
        
        public long Caster;
        public int StartFrame;
        public int IntervalFrame;
        public int EndFrame;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbBuffRow TbBuffRow => this.tbBuffRow ??= TbBuff.Instance.Get(BuffId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbBuffRow tbBuffRow;
    }
}