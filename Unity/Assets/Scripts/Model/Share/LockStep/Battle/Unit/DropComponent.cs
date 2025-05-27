using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class DropComponent : LSEntity, IAwake<int>, ISerializeToEntity
    {
        public int TableId;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbDropRow TbRow => this.tbRow ?? TbDrop.Instance.Get(this.TableId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbDropRow tbRow;
    }
}