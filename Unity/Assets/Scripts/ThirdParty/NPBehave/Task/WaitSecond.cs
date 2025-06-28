using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable]
    public partial class WaitSecond : Task
    {
        [BsonElement][MemoryPackInclude] private FP seconds;
        [BsonElement][MemoryPackInclude] private FP randomVariance;
        
        [MemoryPackConstructor]
        public WaitSecond(FP seconds, FP randomVariance) : base("WaitSecond")
        {
            this.seconds = seconds;
            this.randomVariance = randomVariance;
        }

        public WaitSecond(FP seconds) : base("WaitSecond")
        {
            this.seconds = seconds;
            randomVariance = this.seconds * FP.Ratio(5, 100);
        }

        protected override void DoStart()
        {
            FP delay = seconds;
            if (delay < 0)
            {
                delay = 0;
            }

            if (randomVariance >= FP.Zero)
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