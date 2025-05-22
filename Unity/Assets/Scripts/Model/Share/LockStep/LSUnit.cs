using System;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [ChildOf(typeof(LSUnitComponent))]
    [MemoryPackable]
    public partial class LSUnit: LSEntity, IAwake, ISerializeToEntity
    {
        public int TableId { get; set; }

        public bool Active { get; set; }
        public bool DeadMark { get; set; }
        public bool Dead { get; set; }
    }
}