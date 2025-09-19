using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CardBagComponent: LSEntity, IAwake<List<LSRandomDropItem>>, IDeserialize
    {
        public List<CardBagItem> Items { get; } = new();
        public int ItemIdGenerator;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public Dictionary<int, CardBagItem> IdItemMap { get; } = new();
    }
    
    [EnableClass]
    public class CardBagItem : IPool
    {
        public int Id;
        public EUnitType Type;
        public int TableId;
        public bool IsFromPool { get; set; }
    }
}