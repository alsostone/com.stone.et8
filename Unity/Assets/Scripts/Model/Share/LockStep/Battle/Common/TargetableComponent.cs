using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TargetableComponent : LSEntity, IAwake, ILSUpdate, IDestroy, ISerializeToEntity
    {
        public int ProxyId;
        public TSVector PreviousPosition;
        
        public bool IsDirty { get; set; }
    }
}