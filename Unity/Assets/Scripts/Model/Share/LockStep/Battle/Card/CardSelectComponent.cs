using System;
using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CardSelectComponent: LSEntity, IAwake, ILSUpdate
    {
        public int CurrentSelectCount;
        public List<List<Tuple<EUnitType, int, int>>> CardsQueue { get; private set; } = new ();
    }
}