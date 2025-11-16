using System;
using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CardSelectComponent: LSEntity, IAwake, ILSUpdate, ISerializeToEntity
    {
        public int CurrentSelectCount;
        public List<List<LSRandomDropItem>> CardsQueue { get; set; }
    }
}