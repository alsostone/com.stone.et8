using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class WaitSecond : Task
    {
        [MemoryPackInclude] private readonly float seconds;
        [MemoryPackInclude] private readonly float randomVariance;
        
        [MemoryPackConstructor]
        public WaitSecond(float seconds, float randomVariance) : base("WaitSecond")
        {
            this.seconds = seconds;
            this.randomVariance = randomVariance;
        }

        public WaitSecond(float seconds) : base("WaitSecond")
        {
            this.seconds = seconds;
            randomVariance = this.seconds * 0.05f;
        }

        protected override void DoStart()
        {
            float delay = seconds;
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
    }
}