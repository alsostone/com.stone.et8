using MemoryPack;

namespace NPBehave
{
    public abstract class Service : Decorator
    {
        [MemoryPackInclude] protected readonly float interval = -1.0f;
        [MemoryPackInclude] protected readonly float randomVariation;

        protected Service(float interval, float randomVariation, Node decoratee) : base("Service", decoratee)
        {
            this.interval = interval;
            this.randomVariation = randomVariation;
            Label = "" + (interval - randomVariation) + "..." + (interval + randomVariation) + "s";
        }

        protected Service(float interval, Node decoratee) : base("Service", decoratee)
        {
            this.interval = interval;
            randomVariation = interval * 0.05f;
            Label = "" + (interval - randomVariation) + "..." + (interval + randomVariation) + "s";
        }

        protected Service(Node decoratee) : base("Service", decoratee)
        {
            Label = "every tick";
        }

        protected override void DoStart()
        {
            if (interval <= 0f)
            {
                Clock.AddUpdateObserver(Guid);
                OnService();
            }
            else if (randomVariation <= 0f)
            {
                Clock.AddTimer(interval, -1, Guid);
                OnService();
            }
            else
            {
                Clock.AddTimer(interval, randomVariation, 0, Guid);
                OnService();
            }
            Decoratee.Start();
        }

        protected override void DoStop()
        {
            Decoratee.Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if (interval <= 0f)
            {
                Clock.RemoveUpdateObserver(Guid);
            }
            else if (randomVariation <= 0f)
            {
                Clock.RemoveTimer(Guid);
            }
            else
            {
                Clock.RemoveTimer(Guid);
            }
            Stopped(result);
        }

        public override void OnTimerReached()
        {
            if (interval > 0f && randomVariation > 0f)
            {
                Clock.AddTimer(interval, randomVariation, 0, Guid);
            }
            OnService();
        }
        
        protected abstract void OnService();
    }
}