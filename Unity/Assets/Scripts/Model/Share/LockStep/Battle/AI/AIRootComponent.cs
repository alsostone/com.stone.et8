using MemoryPack;
using NPBehave;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class AIRootComponent : LSEntity, IAwake<Node>, IDestroy, IDeserialize, ISerializeToEntity
    {
        public NPBehave.Root AIRoot;
    }
}