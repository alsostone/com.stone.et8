using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CardSelectComponent: LSEntity, IAwake, ILSUpdate
    {
        public Queue<List<CardItem>> SelectCardQueue { get; private set; } = new Queue<List<CardItem>>();
    }
}