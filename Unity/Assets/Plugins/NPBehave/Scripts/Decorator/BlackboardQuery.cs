using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace NPBehave
{
    public abstract class BlackboardQuery : ObservingDecorator
    {
        [BsonElement][MemoryPackInclude] protected string[] blackboardKeys;

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
        
        public override void OnObservingChanged(BlackboardChangeType type)
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