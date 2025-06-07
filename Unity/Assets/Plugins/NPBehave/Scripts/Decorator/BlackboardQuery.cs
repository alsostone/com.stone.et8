using MemoryPack;

namespace NPBehave
{
    public abstract class BlackboardQuery : ObservingDecorator
    {
        [MemoryPackInclude] protected readonly string[] blackboardKeys;

        protected BlackboardQuery(string[] blackboardKeys, Stops stopsOnChange, Node decoratee) : base("BlackboardQuery", stopsOnChange, decoratee)
        {
            this.blackboardKeys = blackboardKeys;
        }

        protected override void StartObserving()
        {
            foreach (string blackboardKey in blackboardKeys)
            {
                Blackboard.AddObserver(blackboardKey, Guid);
            }
        }

        protected override void StopObserving()
        {
            foreach (string blackboardKey in blackboardKeys)
            {
                Blackboard.RemoveObserver(blackboardKey, Guid);
            }
        }
        
        public override void OnObservingChanged(NotifyType type)
        {
            Evaluate();
        }
        
        public override string ToString()
        {
            string s = "";
            foreach (string key in blackboardKeys)
            {
                s += " " + key;
            }
            return Name + s;
        }
    }
}