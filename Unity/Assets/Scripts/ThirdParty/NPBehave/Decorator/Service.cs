using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    public abstract class Service : Decorator
    {
        [BsonElement][MemoryPackInclude] protected FP interval;
        [BsonElement][MemoryPackInclude] protected FP randomVariation;

        protected Service(FP interval, FP randomVariation, Node decoratee) : base("Service", decoratee)
        {
            this.interval = interval;
            this.randomVariation = randomVariation;
            Label = "" + (interval - randomVariation) + "..." + (interval + randomVariation) + "s";
        }

        protected Service(FP interval, Node decoratee) : base("Service", decoratee)
        {
            this.interval = interval;
            randomVariation = interval * FP.Ratio(5, 100);
            Label = "" + (interval - randomVariation) + "..." + (interval + randomVariation) + "s";
        }

        protected Service(Node decoratee) : base("Service", decoratee)
        {
            interval = -FP.One;
            randomVariation = -FP.One;
            Label = "every tick";
        }

        protected override void DoStart()
        {
            if (interval <= FP.Zero)
            {
                Clock.AddUpdateObserver(Guid);
                OnService();
            }
            else if (randomVariation <= FP.Zero)
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
            if (interval <= FP.Zero)
            {
                Clock.RemoveUpdateObserver(Guid);
            }
            else if (randomVariation <= FP.Zero)
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
            if (interval > FP.Zero && randomVariation > FP.Zero)
            {
                Clock.AddTimer(interval, randomVariation, 0, Guid);
            }
            OnService();
        }
        
        protected abstract void OnService();
    }
}