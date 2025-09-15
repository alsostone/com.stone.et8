using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CardBagComponent: LSEntity, IAwake
    {
        public List<CardItem> Items { get; private set; } = new List<CardItem>();
    }
    
    public struct CardItem
    {
        public EUnitType Type;
        public int TableId;
        public int Count;
    }
}