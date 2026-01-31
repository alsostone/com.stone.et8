using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CardBagComponent: LSEntity, IAwake, IDeserialize, ISerializeToEntity
    {
        public List<CardBagItem> Items { get; set; }

        [BsonIgnore]
        [MemoryPackIgnore]
        public Dictionary<long, CardBagItem> IdItemMap { get; } = new();
    }
    
    [EnableClass]
    [MemoryPackable]
    public partial class CardBagItem : IPool
    {
        public long Id;
        public EUnitType Type;
        public int TableId;
        
        public bool IsFromPool { get; set; }
        
        public CardBagItem CreateCopy()
        {
            CardBagItem copy = ObjectPool.Instance.Fetch<CardBagItem>();
            copy.Id = this.Id;
            copy.Type = this.Type;
            copy.TableId = this.TableId;
            return copy;
        }
    }
}