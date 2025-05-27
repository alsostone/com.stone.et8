using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class SoldierComponent : LSEntity, IAwake<int, int>, ISerializeToEntity
    {
        public int TableId;
        public int Level;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbSoldierRow TbRow => this.tbRow ?? TbSoldier.Instance.Get(this.TableId, this.Level);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbSoldierRow tbRow;
    }
}