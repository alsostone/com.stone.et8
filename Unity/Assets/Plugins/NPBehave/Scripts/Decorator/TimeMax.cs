using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class TimeMax : Decorator
    {
        [MemoryPackInclude] private readonly float limit = 0.0f;
        [MemoryPackInclude] private readonly float randomVariation;
        [MemoryPackInclude] private readonly bool waitForChildButFailOnLimitReached = false;
        [MemoryPackInclude] private bool isLimitReached = false;
     
        public TimeMax(float limit, bool waitForChildButFailOnLimitReached, Node decoratee) : base("TimeMax", decoratee)
        {
            this.limit = limit;
            randomVariation = limit * 0.05f;
            this.waitForChildButFailOnLimitReached = waitForChildButFailOnLimitReached;
        }

        [MemoryPackConstructor]
        public TimeMax(float limit, float randomVariation, bool waitForChildButFailOnLimitReached, Node decoratee) : base("TimeMax", decoratee)
        {
            this.limit = limit;
            this.randomVariation = randomVariation;
            this.waitForChildButFailOnLimitReached = waitForChildButFailOnLimitReached;
        }

        protected override void DoStart()
        {
            isLimitReached = false;
            Clock.AddTimer(limit, randomVariation, 0, Guid);
            Decoratee.Start();
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
            Clock.RemoveTimer(Guid);
            if (isLimitReached)
            {
                Stopped(false);
            }
            else
            {
                Stopped(result);
            }
        }

        public override void OnTimerReached()
        {
            if (!waitForChildButFailOnLimitReached)
            {
                Decoratee.Stop();
            }
            else
            {
                isLimitReached = true;
            }
        }
    }
}