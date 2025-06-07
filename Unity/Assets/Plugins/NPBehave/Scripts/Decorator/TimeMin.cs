using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class TimeMin : Decorator
    {
        [MemoryPackInclude] private readonly float limit = 0.0f;
        [MemoryPackInclude] private readonly float randomVariation;
        [MemoryPackInclude] private readonly bool waitOnFailure = false;
        [MemoryPackInclude] private bool isLimitReached = false;
        [MemoryPackInclude] private bool isDecorateeDone = false;
        [MemoryPackInclude] private bool isDecorateeSuccess = false;
        
        public TimeMin(float limit, Node decoratee) : base("TimeMin", decoratee)
        {
            this.limit = limit;
            randomVariation = this.limit * 0.05f;
            waitOnFailure = false;
        }

        public TimeMin(float limit, bool waitOnFailure, Node decoratee) : base("TimeMin", decoratee)
        {
            this.limit = limit;
            randomVariation = this.limit * 0.05f;
            this.waitOnFailure = waitOnFailure;
        }

        [MemoryPackConstructor]
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
            Clock.AddTimer(limit, randomVariation, 0, Guid);
            Decoratee.Start();
        }

        protected override void DoStop()
        {
            Clock.RemoveTimer(Guid);
            if (Decoratee.IsActive)
            {
                isLimitReached = true;
                Decoratee.Stop();
            }
            else
            {
                Stopped(false);
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            isDecorateeDone = true;
            isDecorateeSuccess = result;
            if (isLimitReached || (!result && !waitOnFailure))
            {
                Clock.RemoveTimer(Guid);
                Stopped(isDecorateeSuccess);
            }
        }

        public override void OnTimerReached()
        {
            isLimitReached = true;
            if (isDecorateeDone)
            {
                Stopped(isDecorateeSuccess);
            }
        }
    }
}