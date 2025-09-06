#if ENABLE_DEBUG
namespace ST.GmTools
{
    internal sealed class DrawViewMgr : IDrawViewMgr
    {
        private readonly DrawViewGroup mViewRoot;
        public bool ActiveWindow { get; set; }
        
        public DrawViewMgr()
        {
            mViewRoot = new DrawViewGroup();
            ActiveWindow = false;
        }
        
        public IDrawViewGroup ViewRoot => mViewRoot;

        public void RegisterDrawView(string path, IDrawView view) => mViewRoot.RegisterDrawView(path, view);

        public bool UnregisterDrawView(string path) => mViewRoot.UnregisterDrawView(path);

        public IDrawView GetDrawView(string path) => mViewRoot.GetDrawView(path);

        public bool SelectDrawView(string path) => mViewRoot.SelectDrawView(path);
    }
}
#endif