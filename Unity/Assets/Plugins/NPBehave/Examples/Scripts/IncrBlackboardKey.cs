using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace NPBehave
{
    [MemoryPackable]
    public partial class IncrBlackboardKey : Node
    {
        [BsonElement][MemoryPackInclude] private string blackboardKey;

        public IncrBlackboardKey(string blackboardKey) : base("IncrBlackboardKey")
        {
            this.blackboardKey = blackboardKey;
        }
        protected override void DoStart()
        {
            Clock.AddUpdateObserver(Guid);
        }
        protected override void DoStop()
        {
            Clock.RemoveUpdateObserver(Guid);
        }
        public override void OnTimerReached()
        {
            Blackboard.SetInt(this.blackboardKey, Blackboard.GetInt(this.blackboardKey) + 1);
        }
    }

}