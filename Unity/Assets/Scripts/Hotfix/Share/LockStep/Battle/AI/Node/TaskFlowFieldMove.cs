using MemoryPack;
using NPBehave;
using TrueSync;

namespace ET
{
    [AINode]
    [MemoryPackable]
    public partial class TaskFlowFieldMove : Task
    {
        public TaskFlowFieldMove() : base("TaskFlowFieldMove")
        {
        }

        protected override void DoStart()
        {
            LSUnit lsUnit = Agent as LSUnit;
            MoveFlowFieldComponent flowFieldComponent = lsUnit.GetComponent<MoveFlowFieldComponent>();
            if (flowFieldComponent.TryMoveStart(0, TSVector.zero, MovementMode.AttackMove))
            {
                Stopped(false);
            }
            else
            {
                Clock.AddTimer(0, 0, -1, Guid);
            }
        }

        protected override void DoStop()
        {
            LSUnit lsUnit = Agent as LSUnit;
            MoveFlowFieldComponent flowFieldComponent = lsUnit.GetComponent<MoveFlowFieldComponent>();
            flowFieldComponent.Stop();
            
            Clock.RemoveTimer(Guid);
            Stopped(false);
        }

        public override void OnTimerReached()
        {
            LSUnit lsUnit = Agent as LSUnit;
            MoveFlowFieldComponent flowFieldComponent = lsUnit.GetComponent<MoveFlowFieldComponent>();
            if (flowFieldComponent.IsArrived())
            {
                Clock.RemoveTimer(Guid);
                Stopped(true);
            }
        }
    }
}