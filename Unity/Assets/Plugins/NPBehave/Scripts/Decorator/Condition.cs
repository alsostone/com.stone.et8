using MemoryPack;

namespace NPBehave
{
    public abstract class Condition : ObservingDecorator
    {
        [MemoryPackInclude] protected float checkInterval;
        [MemoryPackInclude] protected float checkVariance;
        
        protected Condition(Node decoratee) : base("Condition", Stops.NONE, decoratee)
        {
            this.checkInterval = 0.0f;
            this.checkVariance = 0.0f;
        }

        protected Condition(Stops stopsOnChange, Node decoratee) : base("Condition", stopsOnChange, decoratee)
        {
            this.checkInterval = 0.0f;
            this.checkVariance = 0.0f;
        }

        protected Condition(Stops stopsOnChange, float checkInterval, float randomVariance, Node decoratee) : base("Condition", stopsOnChange, decoratee)
        {
            this.checkInterval = checkInterval;
            this.checkVariance = randomVariance;
        }

        protected override void StartObserving()
        {
            this.RootNode.Clock.AddTimer(checkInterval, checkVariance, -1, Evaluate);
        }

        protected override void StopObserving()
        {
            this.RootNode.Clock.RemoveTimer(Evaluate);
        }

    }
}