using MemoryPack;

namespace NPBehave
{
    public abstract class Decorator : Container
    {
        [MemoryPackInclude] protected Node Decoratee;

        protected Decorator(string name, Node decoratee) : base(name)
        {
            Decoratee = decoratee;
            Decoratee.SetParent(this);
        }

        // 防止因为循环依赖导致无法GC
        public override void Dispose()
        {
            Decoratee.Dispose();
            Decoratee = null;
            base.Dispose();
        }

        public override void SetRoot(Root rootNode)
        {
            base.SetRoot(rootNode);
            Decoratee.SetRoot(rootNode);
        }


#if UNITY_EDITOR
        [MemoryPackIgnore] public override Node[] DebugChildren
        {
            get
            {
                return new Node[] { Decoratee };
            }
        }
#endif

        public override void ParentCompositeStopped(Composite composite)
        {
            base.ParentCompositeStopped(composite);
            Decoratee.ParentCompositeStopped(composite);
        }
    }
}