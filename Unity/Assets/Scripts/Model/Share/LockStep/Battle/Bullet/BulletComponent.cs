using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BulletComponent : LSEntity, IAwake<int, LSUnit, LSUnit>, ILSUpdate, ISerializeToEntity
    {
        public int BulletId;
        
        public long Caster;
        public long Target;
        public TSVector TargetPosition;
        public int ElapseFrame;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbBulletRow TbBulletRow => this.tbBulletRow ?? TbBullet.Instance.Get(BulletId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbBulletRow tbBulletRow;
    }
}