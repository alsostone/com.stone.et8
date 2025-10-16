using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class HeroComponent : LSEntity, IAwake<int, int>, IDestroy, IDeserialize, ISerializeToEntity
    {
        public int TableId;
        public int SkinId;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbHeroRow TbRow => this.tbRow ?? TbHero.Instance.Get(this.TableId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbHeroRow tbRow;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbHeroSkinRow TbSkinRow => this.tbSkinRow ?? TbHeroSkin.Instance.Get(this.SkinId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbHeroSkinRow tbSkinRow;
    }
}