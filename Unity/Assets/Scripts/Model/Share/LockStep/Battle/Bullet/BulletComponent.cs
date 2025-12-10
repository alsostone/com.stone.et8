using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BulletComponent : LSEntity, IAwake<int, LSUnit, List<SearchUnit>>, IAwake<int, LSUnit, LSUnit>, IAwake<int, LSUnit>, ILSUpdate, IDestroy, ISerializeToEntity
    {
        public int BulletId;
        public long Caster;
        public int OverFrame;
        
        public long Target;
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