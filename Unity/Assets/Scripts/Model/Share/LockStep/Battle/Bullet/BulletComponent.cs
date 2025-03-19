using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class BulletComponent : LSEntity, IAwake<int, LSUnit, LSUnit>, IDestroy, ILSUpdate
    {
        [BsonIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();

        public int BulletId;
        public EntityRef<LSUnit> Caster;
        public EntityRef<LSUnit> Target;
        public long ElapseTime;
        
        [BsonIgnore]
        public TbSkillBulletRow TbBulletRow => this.tbBulletRow ?? TbSkillBullet.Instance.Get(BulletId);
        [BsonIgnore]
        private TbSkillBulletRow tbBulletRow;
    }
}