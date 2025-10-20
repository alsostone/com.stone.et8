using MemoryPack;
using NPBehave;

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
            Clock.AddTimer(0, 0, -1, Guid);
        }

        protected override void DoStop()
        {
            Clock.RemoveTimer(Guid);
            Stopped(false);
        }

        public override void OnTimerReached()
        {
            LSUnit lsUnit = Agent as LSUnit;
            MoveFlowFieldComponent flowFieldComponent = lsUnit.GetComponent<MoveFlowFieldComponent>();
            flowFieldComponent.DoMove();
        }
    }
}