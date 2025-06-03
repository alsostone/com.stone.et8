using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Root : Decorator
    {
        [MemoryPackInclude] private readonly Blackboard rootBlackboard;
        [MemoryPackIgnore]
        public Blackboard RootBlackboard
        {
            get
            {
                return this.rootBlackboard;
            }
        }
        
        [MemoryPackInclude] private readonly Clock rootClock;
        [MemoryPackIgnore]
        public Clock RootClock
        {
            get
            {
                return this.rootClock;
            }
        }

#if UNITY_EDITOR
        [MemoryPackIgnore] public int TotalNumStartCalls = 0;
        [MemoryPackIgnore] public int TotalNumStopCalls = 0;
        [MemoryPackIgnore] public int TotalNumStoppedCalls = 0;
#endif
        
        public Root(Clock rootClock, Node decoratee) : base("Root", decoratee)
        {
            this.rootBlackboard = new Blackboard(rootClock);
            this.rootClock = rootClock;
            this.SetRoot(this);
        }

        [MemoryPackConstructor]
        public Root(Blackboard rootBlackboard, Clock rootClock, Node decoratee) : base("Root", decoratee)
        {
            this.rootBlackboard = rootBlackboard;
            this.rootClock = rootClock;
            this.SetRoot(this);
        }
        
        public sealed override void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);
        }

        protected override void DoStart()
        {
            this.rootBlackboard.Enable();
            this.Decoratee.Start();
        }

        protected override void DoStop()
        {
            if (this.Decoratee.IsActive)
            {
                this.Decoratee.Stop();
            }
            else
            {
                this.rootClock.RemoveTimer(this.Decoratee.Start);
            }
        }
        
        protected override void DoChildStopped(Node node, bool success)
        {
            if (!IsStopRequested)
            {
                // wait one tick, to prevent endless recursions
                this.rootClock.AddTimer(0, 0, this.Decoratee.Start);
            }
            else
            {
                this.rootBlackboard.Disable();
                Stopped(success);
            }
        }
    }
}
