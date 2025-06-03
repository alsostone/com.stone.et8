using MemoryPack;

namespace NPBehave
{
    public abstract class WaitForCondition : Decorator
    {
        [MemoryPackInclude] protected readonly float checkInterval;
        [MemoryPackInclude] protected readonly float checkVariance;
        
        protected WaitForCondition(float checkInterval, float randomVariance, Node decoratee) : base("WaitForCondition", decoratee)
        {
            this.checkInterval = checkInterval;
            this.checkVariance = randomVariance;
            this.Label = "" + (checkInterval - randomVariance) + "..." + (checkInterval + randomVariance) + "s";
        }

        protected WaitForCondition(Node decoratee) : base("WaitForCondition", decoratee)
        {
            this.checkInterval = 0.0f;
            this.checkVariance = 0.0f;
            this.Label = "every tick";
        }

        protected override void DoStart()
        {
            if (!this.IsConditionMet())
            {
                Clock.AddTimer(checkInterval, checkVariance, -1, this.OnTimer);
            }
            else
            {
                Decoratee.Start();
            }
        }

        private void OnTimer()
        {
            if (this.IsConditionMet())
            {
                Clock.RemoveTimer(this.OnTimer);
                Decoratee.Start();
            }
        }

        protected override void DoStop()
        {
            Clock.RemoveTimer(this.OnTimer);
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
            Stopped(result);
        }

        protected abstract bool IsConditionMet();
    }
}