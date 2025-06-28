using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable]
    public partial class WaitBlackboardKey : Task
    {
        [BsonElement][MemoryPackInclude] private string blackboardKey = null;
        [BsonElement][MemoryPackInclude] private FP randomVariance;
        
        public WaitBlackboardKey(string blackboardKey) : base("WaitBlackboardKey")
        {
            this.blackboardKey = blackboardKey;
            this.randomVariance = FP.Zero;
        }
        
        [MemoryPackConstructor]
        public WaitBlackboardKey(string blackboardKey, FP randomVariance) : base("WaitBlackboardKey")
        {
            this.blackboardKey = blackboardKey;
            this.randomVariance = randomVariance;
        }

        protected override void DoStart()
        {
            FP delay = Blackboard.GetFloat(blackboardKey);
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