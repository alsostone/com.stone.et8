using MemoryPack;

namespace NPBehave
{
    public abstract class BlackboardQuery : ObservingDecorator
    {
        [MemoryPackInclude] protected readonly string[] keys;

        protected BlackboardQuery(string[] keys, Stops stopsOnChange, Node decoratee) : base("BlackboardQuery", stopsOnChange, decoratee)
        {
            this.keys = keys;
        }

        protected override void StartObserving()
        {
            foreach (string key in this.keys)
            {
                Blackboard.AddObserver(key, onValueChanged);
            }
        }

        protected override void StopObserving()
        {
            foreach (string key in this.keys)
            {
                Blackboard.RemoveObserver(key, onValueChanged);
            }
        }

        private void onValueChanged(Blackboard.Type type, object newValue)
        {
            Evaluate();
        }

        public override string ToString()
        {
            string keys = "";
            foreach (string key in this.keys)
            {
                keys += " " + key;
            }
            return Name + keys;
        }
    }
}