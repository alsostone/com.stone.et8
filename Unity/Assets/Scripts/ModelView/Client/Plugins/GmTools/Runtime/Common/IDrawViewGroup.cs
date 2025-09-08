#if ENABLE_DEBUG
namespace ST.GmTools
{
    public interface IDrawViewGroup : IDrawView
    {
        int DrawViewCount { get; }

        string[] GetDrawViewNames();

        IDrawView GetDrawView(string path);

        void RegisterDrawView(string path, IDrawView view);
    }
}
#endif