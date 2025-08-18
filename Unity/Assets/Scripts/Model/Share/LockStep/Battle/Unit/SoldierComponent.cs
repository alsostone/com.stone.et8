using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class SoldierComponent : LSEntity, IAwake<int>, ISerializeToEntity
    {
        public int TableId;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbSoldierRow TbRow => this.tbRow ?? TbSoldier.Instance.Get(this.TableId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbSoldierRow tbRow;
    }
}