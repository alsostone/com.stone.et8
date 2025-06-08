using MemoryPack;
using TrueSync;

namespace NPBehave
{
    public abstract class Condition : ObservingDecorator
    {
        [MemoryPackInclude] protected FP checkInterval;
        [MemoryPackInclude] protected FP checkVariance;
        
        protected Condition(Node decoratee) : base("Condition", Stops.NONE, decoratee)
        {
            checkInterval = FP.Zero;
            checkVariance = FP.Zero;
        }

        protected Condition(Stops stopsOnChange, Node decoratee) : base("Condition", stopsOnChange, decoratee)
        {
            checkInterval = FP.Zero;
            checkVariance = FP.Zero;
        }

        protected Condition(Stops stopsOnChange, FP checkInterval, FP randomVariance, Node decoratee) : base("Condition", stopsOnChange, decoratee)
        {
            this.checkInterval = checkInterval;
            checkVariance = randomVariance;
        }

        protected override void StartObserving()
        {
            Clock.AddTimer(checkInterval, checkVariance, -1, Guid);
        }

        protected override void StopObserving()
        {
            Clock.RemoveTimer(Guid);
        }

        public override void OnTimerReached()
        {
            Evaluate();
        }
    }
}