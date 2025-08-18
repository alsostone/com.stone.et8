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
            Blackboard.AddObserver(AIConstValue.HasEnemy, Guid);
            Clock.AddTimer(0, 0, -1, Guid);
        }

        protected override void DoStop()
        {
            StopAndCleanUp(false);
        }

        public override void OnObservingChanged(BlackboardChangeType type)
        {
            if (!Blackboard.GetBool(AIConstValue.HasEnemy))
            {
                StopAndCleanUp(false);
            }
        }

        public override void OnTimerReached()
        {
            LSUnit lsUnit = Agent as LSUnit;
            FieldMoveComponent fieldMoveComponent = lsUnit.GetComponent<FieldMoveComponent>();
            fieldMoveComponent.DoMove();
        }

        private void StopAndCleanUp(bool result)
        {
            Blackboard.RemoveObserver(AIConstValue.HasEnemy, Guid);
            Clock.RemoveTimer(Guid);
            Stopped(result);
        }
    }
}