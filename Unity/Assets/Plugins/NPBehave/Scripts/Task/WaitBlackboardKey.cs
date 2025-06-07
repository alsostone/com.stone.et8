using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class WaitBlackboardKey : Task
    {
        [MemoryPackInclude] private readonly string blackboardKey = null;
        [MemoryPackInclude] private readonly float randomVariance;
        
        public WaitBlackboardKey(string blackboardKey, float randomVariance = 0f) : base("WaitBlackboardKey")
        {
            this.blackboardKey = blackboardKey;
            this.randomVariance = randomVariance;
        }
        
        protected override void DoStart()
        {
            float delay = Blackboard.GetFloat(blackboardKey);
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