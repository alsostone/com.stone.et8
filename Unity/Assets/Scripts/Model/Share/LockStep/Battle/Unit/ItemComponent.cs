using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class ItemComponent : LSEntity, IAwake<int>, ISerializeToEntity
    {
        public int TableId;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbItemRow TbRow => this.tbRow ?? TbItem.Instance.Get(this.TableId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbItemRow tbRow;
    }
}