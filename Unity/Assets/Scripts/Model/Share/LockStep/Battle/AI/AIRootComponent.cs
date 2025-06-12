using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class AIRootComponent : LSEntity, IAwake<EUnitType>, IDestroy, IDeserialize, ISerializeToEntity
    {
        public NPBehave.Root AIRoot;
    }
}