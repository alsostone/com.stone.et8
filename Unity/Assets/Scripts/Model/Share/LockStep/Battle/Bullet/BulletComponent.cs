using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BulletComponent : LSEntity, IAwake<int, LSUnit, LSUnit>, IDestroy, ILSUpdate, ISerializeToEntity
    {
        [BsonIgnore]
        [MemoryPackIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();

        public int BulletId;
        public long ElapseTime;
        
        public long Caster;
        public long Target;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbSkillBulletRow TbBulletRow => this.tbBulletRow ?? TbSkillBullet.Instance.Get(BulletId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbSkillBulletRow tbBulletRow;
    }
}