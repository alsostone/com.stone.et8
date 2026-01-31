using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class ContainerComponent : LSEntity, IAwake, ISerializeToEntity
    {
        public List<long> Contents;
    }
}