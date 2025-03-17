using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(BuffComponent))]
    public class Buff : LSEntity, IAwake<int>, IDestroy, ILSUpdate
    {
        [BsonIgnore]
        public LSUnit Owner => this.GetParent<BuffComponent>().Owner;

        public int BuffId;
        public long StartTime;
        public long IntervalTime;
        public uint LayerCount;
        
        public EntityRef<LSUnit> Caster;
        
        [BsonIgnore]
        public TbSkillBuffRow TbBuffRow => this.tbBuffRow ??= TbSkillBuff.Instance.Get(BuffId);
        [BsonIgnore]
        private TbSkillBuffRow tbBuffRow;
    }
}