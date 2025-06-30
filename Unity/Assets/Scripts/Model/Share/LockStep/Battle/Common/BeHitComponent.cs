using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class BeHitComponent : LSEntity, IAwake, IDestroy, IDeserialize, ISerializeToEntity
    {
        public List<long> Attackers = new();
    }
}