using MemoryPack;

namespace NPBehave
{
    public abstract class WaitCalc : Task
    {
        [MemoryPackInclude] private readonly float randomVariance;
        
        protected WaitCalc(float randomVariance = 0f) : base("WaitCalc")
        {
            this.randomVariance = randomVariance;
        }

        protected override void DoStart()
        {
            float delay = CalcSeconds();
            if (delay < 0)
            {
                delay = 0;
            }

            if (randomVariance >= 0f)
            {
                Clock.AddTimer(delay, randomVariance, 0, Guid);
            }
            else
            {
                Clock.AddTimer(delay, 0, Guid);
            }
        }

        protected override void DoStop()
        {
            Clock.RemoveTimer(Guid);
            Stopped(false);
        }

        public override void OnTimerReached()
        {
            Clock.RemoveTimer(Guid);
            Stopped(true);
        }

        protected abstract float CalcSeconds();
    }
}