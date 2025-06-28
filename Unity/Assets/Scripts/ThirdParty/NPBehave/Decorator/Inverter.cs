using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Inverter : Decorator
    {
        public Inverter(Node decoratee) : base("Inverter", decoratee)
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
            Stopped(!result);
        }
    }
}