using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class MoveFlowFieldComponent : LSEntity, IAwake, ILSUpdate, IDestroy, ISerializeToEntity
    {
        public int FlowFieldIndex;
        public TSVector FlowFieldDestination;
        public FlowFieldStatus FlowFieldStatus;
        public MovementMode MovementMode;
    }
    
    public enum FlowFieldStatus
    {
        None = 0,
        Moving = 1,
        Arrived = 2,
    }
}