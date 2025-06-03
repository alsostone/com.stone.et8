using MemoryPack;

namespace NPBehave
{
    public abstract class Composite : Container
    {
        [MemoryPackInclude] protected Node[] Children;

        protected Composite(string name, Node[] children) : base(name)
        {
            this.Children = children;
            foreach (Node node in Children)
            {
                node.SetParent(this);
            }
        }

        public override void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);

            foreach (Node node in Children)
            {
                node.SetRoot(rootNode);
            }
        }


#if UNITY_EDITOR
        [MemoryPackIgnore] public override Node[] DebugChildren
        {
            get
            {
                return this.Children;
            }
        }

        public Node DebugGetActiveChild()
        {
            foreach( Node node in DebugChildren )
            {
                if(node.CurrentState == Node.State.ACTIVE )
                {
                    return node;
                }
            }

            return null;
        }
#endif

        protected override void Stopped(bool success)
        {
            foreach (Node child in Children)
            {
                child.ParentCompositeStopped(this);
            }
            base.Stopped(success);
        }

        public abstract void StopLowerPriorityChildrenForChild(Node child, bool immediateRestart);
    }
}