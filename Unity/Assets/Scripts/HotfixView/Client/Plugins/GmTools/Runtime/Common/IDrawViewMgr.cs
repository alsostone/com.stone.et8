#if ENABLE_DEBUG
namespace ST.GmTools
{
    public interface IDrawViewMgr
    {
        bool ActiveWindow { get; set; }

        IDrawViewGroup ViewRoot { get; }

        void RegisterDrawView(string path, IDrawView view);

        bool UnregisterDrawView(string path);

        IDrawView GetDrawView(string path);

        bool SelectDrawView(string path);
    }
}
#endif