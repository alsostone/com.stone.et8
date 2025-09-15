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
        public Queue<List<Tuple<EUnitType, int, int>>> SelectCardQueue { get; private set; } = new Queue<List<Tuple<EUnitType, int, int>>>();
    }
}