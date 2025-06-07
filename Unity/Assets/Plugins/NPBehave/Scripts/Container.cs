using MemoryPack;

namespace NPBehave
{
    public abstract class Container : Node
    {
        [MemoryPackInclude] protected bool collapse = false;
        
        [MemoryPackIgnore]
        public bool Collapse
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
        [MemoryPackIgnore] public abstract Node[] DebugChildren
        {
            get;
        }
#endif
    }
}