using System;
using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Root : Decorator
    {
        [MemoryPackInclude] private readonly int blackboardGuid;
        
        [MemoryPackIgnore] public Blackboard RootBlackboard { get; private set; }
        [MemoryPackIgnore] public Clock RootClock { get; private set; }
        [MemoryPackIgnore] public BehaveWorld RootBehaveWorld { get; private set; }

#if UNITY_EDITOR
        [MemoryPackIgnore] public int TotalNumStartCalls = 0;
        [MemoryPackIgnore] public int TotalNumStopCalls = 0;
        [MemoryPackIgnore] public int TotalNumStoppedCalls = 0;
#endif
        
        [MemoryPackConstructor]
        private Root(Node decoratee) : base("Root", decoratee)
        {
        }

        public Root(BehaveWorld world, Node decoratee) : base("Root", decoratee)
        {
            RootBlackboard = world.CreateBlackboard();
            RootClock = world.Clock;
            RootBehaveWorld = world;
            blackboardGuid = RootBlackboard.Guid;
            base.SetRoot(this);
        }
        
        public Root(BehaveWorld world, Blackboard blackboard, Node decoratee) : base("Root", decoratee)
        {
            RootBlackboard = blackboard;
            RootClock = world.Clock;
            RootBehaveWorld = world;
            blackboardGuid = RootBlackboard.Guid;
            base.SetRoot(this);
        }
        
        // 反序列化后 手动调用该接口 以恢复上下文
        public void SetWorld(BehaveWorld world)
        {
            RootBlackboard = world.GetBlackboard(blackboardGuid);
            RootClock = world.Clock;
            RootBehaveWorld = world;
            base.SetRoot(this);
        }

        protected override void DoStart()
        {
            Blackboard.Enable();
            Decoratee.Start();
        }

        protected override void DoStop()
        {
            if (Decoratee.IsActive)
            {
                Decoratee.Stop();
            }
            else
            {
                RootClock.RemoveTimer(Guid);
            }
        }
        
        protected override void DoChildStopped(Node node, bool success)
        {
            if (!IsStopRequested)
            {
                // wait one tick, to prevent endless recursions
                Clock.AddTimer(0, 0, Guid);
            }
            else
            {
                Blackboard.Disable();
                Stopped(success);
            }
        }

        public override void OnTimerReached()
        {
            Decoratee.Start();
        }
    }
}
