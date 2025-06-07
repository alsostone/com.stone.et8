using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Repeater : Decorator
    {
        [MemoryPackInclude] private readonly int loopCount = -1;
        [MemoryPackInclude] private int currentLoop;
        
        /// <param name="loopCount">number of times to execute the decoratee. Set to -1 to repeat forever, be careful with endless loops!</param>
        /// <param name="decoratee">Decorated Node</param>
        [MemoryPackConstructor]
        public Repeater(int loopCount, Node decoratee) : base("Repeater", decoratee)
        {
            this.loopCount = loopCount;
        }

        /// <param name="decoratee">Decorated Node, repeated forever</param>
        public Repeater(Node decoratee) : base("Repeater", decoratee)
        {
        }

        protected override void DoStart()
        {
            if (loopCount != 0)
            {
                currentLoop = 0;
                Decoratee.Start();
            }
            else
            {
                Stopped(true);
            }
        }

        protected override void DoStop()
        {
            Clock.RemoveTimer(Guid);
            
            if (Decoratee.IsActive)
            {
                Decoratee.Stop();
            }
            else
            {
                Stopped(false);
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if (result)
            {
                if (IsStopRequested || (loopCount > 0 && ++currentLoop >= loopCount))
                {
                    Stopped(true);
                }
                else
                {
                    Clock.AddTimer(0, 0, Guid);
                }
            }
            else
            {
                Stopped(false);
            }
        }

        public override void OnTimerReached()
        {
            Decoratee.Start();
        }
    }
}