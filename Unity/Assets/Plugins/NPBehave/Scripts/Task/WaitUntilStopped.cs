using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class WaitUntilStopped : Task
    {
        [MemoryPackInclude] private readonly bool successWhenStopped;
        
        public WaitUntilStopped(bool successWhenStopped = false) : base("WaitUntilStopped")
        {
            this.successWhenStopped = successWhenStopped;
        }

        protected override void DoStop()
        {
            Stopped(successWhenStopped);
        }
    }
}