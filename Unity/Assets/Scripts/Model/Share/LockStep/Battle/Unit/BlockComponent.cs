using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BlockComponent : LSEntity, IAwake<int>, ISerializeToEntity
    {
        public int TableId;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbBlockRow TbRow => this.tbRow ?? TbBlock.Instance.Get(this.TableId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbBlockRow tbRow;
    }
}