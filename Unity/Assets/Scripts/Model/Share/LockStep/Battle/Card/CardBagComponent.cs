using System;
using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CardBagComponent: LSEntity, IAwake
    {
        public List<Tuple<EUnitType, int, int>> Items { get; private set; } = new ();
    }
}