using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(BuffComponent))]
    public class Buff : Entity, IAwake<int>, IDestroy
    {
        [BsonIgnore]
        public Unit Unit => this.GetParent<Unit>();

        public int BuffId;
        public long StartTime;
        public long IntervalTime;
        public uint LayerCount;

        [BsonIgnore]
        public TbSkillBuffRow TbBuffRow => this.tbBuffRow ??= TbSkillBuff.Instance.Get(BuffId);
        [BsonIgnore]
        private TbSkillBuffRow tbBuffRow;
    }
}