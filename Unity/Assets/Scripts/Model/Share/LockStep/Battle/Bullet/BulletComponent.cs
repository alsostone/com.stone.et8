using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BulletComponent : LSEntity, IAwake<int, LSUnit, LSUnit>, IAwake<int, LSUnit, FP, List<SearchUnit>>, ILSUpdate, IDestroy, ISerializeToEntity
    {
        public int BulletId;
        
        public long Caster;
        public long Target;
        public int OverFrame;
        public List<SearchUnitPackable> SearchUnits;
        public int HitSearchIndex = 0;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbBulletRow TbBulletRow => this.tbBulletRow ?? TbBullet.Instance.Get(BulletId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbBulletRow tbBulletRow;
    }
}