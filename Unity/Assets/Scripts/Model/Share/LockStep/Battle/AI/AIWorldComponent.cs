using MemoryPack;
using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(LSWorld))]
    [MemoryPackable]
    public partial class AIWorldComponent : LSEntity, IAwake, IDestroy, ILSUpdate, IDeserialize, ISerializeToEntity
    {
        public NPBehave.BehaveWorld BehaveWorld;
        public List<long> NeedStartUnits { get; set; }
    }
}