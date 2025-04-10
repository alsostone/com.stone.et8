using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class HeroComponent : LSEntity, IAwake<int>, IDestroy, ILSUpdate, ISerializeToEntity
    {
        public int TableId;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbHeroRow TbRow => this.tbRow ?? TbHero.Instance.Get(this.TableId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbHeroRow tbRow;
    }
}