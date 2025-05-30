﻿using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BuildingComponent : LSEntity, IAwake<int, int>, ISerializeToEntity
    {
        public int TableId;
        public int Level;

        [BsonIgnore]
        [MemoryPackIgnore]
        public TbBuildingRow TbRow => this.tbRow ?? TbBuilding.Instance.Get(this.TableId, this.Level);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbBuildingRow tbRow;
    }
}