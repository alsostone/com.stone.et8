using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Random : Decorator
    {
        // [1~10000]
        [MemoryPackInclude] private readonly int probability;
        
        public Random(int probability, Node decoratee) : base("Random", decoratee)
        {
            this.probability = probability;
        }

        protected override void DoStart()
        {
            if (RootNode.RootBehaveWorld.GetRandomNext(10000) < probability)
            {
                Decoratee.Start();
            }
            else
            {
                Stopped(false);
            }
        }

        protected override void DoStop()
        {
            Decoratee.Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            Stopped(result);
        }
    }
}