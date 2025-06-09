using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable]
    public partial class TimeMin : Decorator
    {
        [BsonElement][MemoryPackInclude] private FP limit = FP.Zero;
        [BsonElement][MemoryPackInclude] private FP randomVariation;
        [BsonElement][MemoryPackInclude] private bool waitOnFailure = false;
        [BsonElement][MemoryPackInclude] private bool isLimitReached = false;
        [BsonElement][MemoryPackInclude] private bool isDecorateeDone = false;
        [BsonElement][MemoryPackInclude] private bool isDecorateeSuccess = false;
        
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