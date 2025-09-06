#if ENABLE_DEBUG
using UnityEditor;

namespace ST.GmTools
{
    public class GmToolsWindow : EditorWindow
    {
        [MenuItem("ET/Open GmTools &g", false, 20)]
        public static void OpenGmToolsWindow()
        {
            var window = GetWindow<GmToolsWindow>(false, "GmTools", true);
            if (null != window) {
                window.Show();
            }
        }
        
        private GmToolsView mView;
        private void Awake()
        {
            mView = new GmToolsView(null);
        }

        private void OnGUI()
        {
            // 代码编译完成后局部变量被清空
            if (mView == null)
                mView = new GmToolsView(null);
            
            mView.OnGUI();
        }
    }
}
#endif