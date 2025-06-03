using MemoryPack;
using UnityEngine.Assertions;

namespace NPBehave
{
    public abstract class Container : Node
    {
        [MemoryPackInclude] protected bool collapse = false;
        
        [MemoryPackIgnore]
        public bool Collapse
        {
            get
            {
                return collapse;
            }
            set
            {
                collapse = value;
            }
        }

        protected Container(string name) : base(name)
        {
        }

        public void ChildStopped(Node child, bool succeeded)
        {
            // Assert.AreNotEqual(this.currentState, State.INACTIVE, "The Child " + child.Name + " of Container " + this.Name + " was stopped while the container was inactive. PATH: " + GetPath());
            Assert.AreNotEqual(this.currentState, State.INACTIVE, "A Child of a Container was stopped while the container was inactive.");
            this.DoChildStopped(child, succeeded);
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