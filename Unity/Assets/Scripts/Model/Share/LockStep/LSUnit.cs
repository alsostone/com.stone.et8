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
        public int DeadMark { get; set; }   // 0未死 1血量归零 2释放死亡技能中 3死亡
    }
}