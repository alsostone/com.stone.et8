using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BuildingComponent : LSEntity, IAwake<int>, ISerializeToEntity
    {
        public int TableId;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbBuildingRow TbRow => this.tbRow ?? TbBuilding.Instance.Get(this.TableId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbBuildingRow tbRow;
    }
}