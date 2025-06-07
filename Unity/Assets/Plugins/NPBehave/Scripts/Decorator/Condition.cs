﻿using MemoryPack;

namespace NPBehave
{
    public abstract class Condition : ObservingDecorator
    {
        [MemoryPackInclude] protected float checkInterval;
        [MemoryPackInclude] protected float checkVariance;
        
        protected Condition(Node decoratee) : base("Condition", Stops.NONE, decoratee)
        {
            checkInterval = 0.0f;
            checkVariance = 0.0f;
        }

        protected Condition(Stops stopsOnChange, Node decoratee) : base("Condition", stopsOnChange, decoratee)
        {
            checkInterval = 0.0f;
            checkVariance = 0.0f;
        }

        protected Condition(Stops stopsOnChange, float checkInterval, float randomVariance, Node decoratee) : base("Condition", stopsOnChange, decoratee)
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