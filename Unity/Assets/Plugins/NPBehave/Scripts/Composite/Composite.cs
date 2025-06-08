using MemoryPack;

namespace NPBehave
{
    public abstract class Composite : Container
    {
        [MemoryPackInclude] protected readonly Node[] Children;

        protected Composite(string name, Node[] children) : base(name)
        {
            Children = children;
            foreach (Node node in Children)
            {
                node.SetParent(this);
            }
        }

        // 防止因为循环依赖导致无法GC
        public override void Dispose()
        {
            for (int i = 0; i < Children.Length; i++)
            {
                Children[i].Dispose();
                Children[i] = null;
            }
            base.Dispose();
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
                return Children;
            }
        }

        public Node DebugGetActiveChild()
        {
            foreach( Node node in DebugChildren )
            {
                if(node.CurrentState == State.ACTIVE )
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