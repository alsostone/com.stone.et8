using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Observer : Decorator
    {
        public Observer(Node decoratee) : base("Observer", decoratee)
        {
        }

        protected override void DoStart()
        {
            // do something
            Decoratee.Start();
        }

        protected override void DoStop()
        {
            Decoratee.Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            // do something
            Stopped(result);
        }
    }
}