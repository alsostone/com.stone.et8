using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSWorld))]
    [MemoryPackable]
    public partial class AIWorldComponent : LSEntity, IAwake, IDestroy, ILSUpdate, IDeserialize, ISerializeToEntity
    {
        public NPBehave.BehaveWorld BehaveWorld;
    }
}