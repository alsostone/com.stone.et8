using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class TimeMin : Decorator
    {
        [MemoryPackInclude] private float limit = 0.0f;
        [MemoryPackInclude] private float randomVariation;
        [MemoryPackInclude] private bool waitOnFailure = false;
        [MemoryPackInclude] private bool isLimitReached = false;
        [MemoryPackInclude] private bool isDecorateeDone = false;
        [MemoryPackInclude] private bool isDecorateeSuccess = false;
        
        [MemoryPackConstructor]
        public TimeMin(float limit, Node decoratee) : base("TimeMin", decoratee)
        {
            this.limit = limit;
            this.randomVariation = this.limit * 0.05f;
            this.waitOnFailure = false;
        }

        public TimeMin(float limit, bool waitOnFailure, Node decoratee) : base("TimeMin", decoratee)
        {
            this.limit = limit;
            this.randomVariation = this.limit * 0.05f;
            this.waitOnFailure = waitOnFailure;
        }

        public TimeMin(float limit, float randomVariation, bool waitOnFailure, Node decoratee) : base("TimeMin", decoratee)
        {
            this.limit = limit;
            this.randomVariation = randomVariation;
            this.waitOnFailure = waitOnFailure;
        }

        protected override void DoStart()
        {
            isDecorateeDone = false;
            isDecorateeSuccess = false;
            isLimitReached = false;
            Clock.AddTimer(limit, randomVariation, 0, TimeoutReached);
            Decoratee.Start();
        }

        protected override void DoStop()
        {
            if (Decoratee.IsActive)
            {
                Clock.RemoveTimer(TimeoutReached);
                isLimitReached = true;
                Decoratee.Stop();
            }
            else
            {
                Clock.RemoveTimer(TimeoutReached);
                Stopped(false);
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            isDecorateeDone = true;
            isDecorateeSuccess = result;
            if (isLimitReached || (!result && !waitOnFailure))
            {
                Clock.RemoveTimer(TimeoutReached);
                Stopped(isDecorateeSuccess);
            }
            else
            {
                Clock.HasTimer(TimeoutReached);
            }
        }

        private void TimeoutReached()
        {
            isLimitReached = true;
            if (isDecorateeDone)
            {
                Stopped(isDecorateeSuccess);
            }
        }
    }
}