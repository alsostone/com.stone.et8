using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class IncrBlackboardKey : Node
    {
        [MemoryPackInclude] private string key;

        public IncrBlackboardKey(string key) : base("IncrBlackboardKey")
        {
            this.key = key;
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
            Blackboard.SetInt(key, Blackboard.GetInt(key) + 1);
        }
    }

}