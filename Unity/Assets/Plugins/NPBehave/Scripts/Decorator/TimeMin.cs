using MemoryPack;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable]
    public partial class TimeMin : Decorator
    {
        [MemoryPackInclude] private readonly FP limit = FP.Zero;
        [MemoryPackInclude] private readonly FP randomVariation;
        [MemoryPackInclude] private readonly bool waitOnFailure = false;
        [MemoryPackInclude] private bool isLimitReached = false;
        [MemoryPackInclude] private bool isDecorateeDone = false;
        [MemoryPackInclude] private bool isDecorateeSuccess = false;
        
        public TimeMin(FP limit, Node decoratee) : base("TimeMin", decoratee)
        {
            this.limit = limit;
            randomVariation = this.limit * FP.Ratio(5, 100);
            waitOnFailure = false;
        }

        public TimeMin(FP limit, bool waitOnFailure, Node decoratee) : base("TimeMin", decoratee)
        {
            this.limit = limit;
            randomVariation = this.limit * FP.Ratio(5, 100);
            this.waitOnFailure = waitOnFailure;
        }

        [MemoryPackConstructor]
        public TimeMin(FP limit, FP randomVariation, bool waitOnFailure, Node decoratee) : base("TimeMin", decoratee)
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