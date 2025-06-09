using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace NPBehave
{
    public abstract class Container : Node
    {
        [BsonElement][MemoryPackInclude] protected bool collapse = false;
        
        [BsonIgnore][MemoryPackIgnore] public bool Collapse
        {
            get => collapse;
            set => collapse = value;
        }

        protected Container(string name) : base(name)
        {
        }

        public void ChildStopped(Node child, bool succeeded)
        {
            DoChildStopped(child, succeeded);
        }

        protected abstract void DoChildStopped(Node child, bool succeeded);

#if UNITY_EDITOR
        [BsonIgnore][MemoryPackIgnore] public abstract Node[] DebugChildren
        {
            get;
        }
#endif
    }
}