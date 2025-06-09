using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable]
    public partial class TimeMax : Decorator
    {
        [BsonElement][MemoryPackInclude] private FP limit = FP.Zero;
        [BsonElement][MemoryPackInclude] private FP randomVariation;
        [BsonElement][MemoryPackInclude] private bool waitForChildButFailOnLimitReached = false;
        [BsonElement][MemoryPackInclude] private bool isLimitReached = false;
     
        public TimeMax(FP limit, bool waitForChildButFailOnLimitReached, Node decoratee) : base("TimeMax", decoratee)
        {
            this.limit = limit;
            randomVariation = limit * FP.Ratio(5, 100);
            this.waitForChildButFailOnLimitReached = waitForChildButFailOnLimitReached;
        }

        [MemoryPackConstructor]
        public TimeMax(FP limit, FP randomVariation, bool waitForChildButFailOnLimitReached, Node decoratee) : base("TimeMax", decoratee)
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