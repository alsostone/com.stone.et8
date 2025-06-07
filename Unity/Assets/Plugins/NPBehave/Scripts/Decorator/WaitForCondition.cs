﻿using MemoryPack;

namespace NPBehave
{
    public abstract class WaitForCondition : Decorator
    {
        [MemoryPackInclude] protected readonly float checkInterval;
        [MemoryPackInclude] protected readonly float checkVariance;
        
        protected WaitForCondition(float checkInterval, float randomVariance, Node decoratee) : base("WaitForCondition", decoratee)
        {
            this.checkInterval = checkInterval;
            checkVariance = randomVariance;
            Label = "" + (checkInterval - randomVariance) + "..." + (checkInterval + randomVariance) + "s";
        }

        protected WaitForCondition(Node decoratee) : base("WaitForCondition", decoratee)
        {
            checkInterval = 0.0f;
            checkVariance = 0.0f;
            Label = "every tick";
        }

        protected override void DoStart()
        {
            if (!IsConditionMet())
            {
                Clock.AddTimer(checkInterval, checkVariance, -1, Guid);
            }
            else
            {
                Decoratee.Start();
            }
        }

        public override void OnTimerReached()
        {
            if (IsConditionMet())
            {
                Clock.RemoveTimer(Guid);
                Decoratee.Start();
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
            Stopped(result);
        }
        
        protected abstract bool IsConditionMet();
    }
}