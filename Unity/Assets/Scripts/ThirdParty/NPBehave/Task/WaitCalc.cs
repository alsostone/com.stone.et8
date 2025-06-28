using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    public abstract class WaitCalc : Task
    {
        [BsonElement][MemoryPackInclude] private FP randomVariance;
        
        protected WaitCalc() : base("WaitCalc")
        {
            this.randomVariance = FP.Zero;
        }
        
        [MemoryPackConstructor]
        protected WaitCalc(FP randomVariance) : base("WaitCalc")
        {
            this.randomVariance = randomVariance;
        }

        protected override void DoStart()
        {
            FP delay = CalcSeconds();
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

        protected abstract FP CalcSeconds();
    }
}