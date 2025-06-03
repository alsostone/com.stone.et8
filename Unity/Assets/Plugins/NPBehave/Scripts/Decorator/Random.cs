using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Random : Decorator
    {
        [MemoryPackInclude] private readonly float probability;
        
        public Random(float probability, Node decoratee) : base("Random", decoratee)
        {
            this.probability = probability;
        }

        protected override void DoStart()
        {
            if (UnityEngine.Random.value <= this.probability)
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