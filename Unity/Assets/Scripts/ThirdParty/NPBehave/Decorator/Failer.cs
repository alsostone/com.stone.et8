using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Failer : Decorator
    {
        public Failer(Node decoratee) : base("Failer", decoratee)
        {
        }

        protected override void DoStart()
        {
            Decoratee.Start();
        }

        protected override void DoStop()
        {
            Decoratee.Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            Stopped(false);
        }
    }

}