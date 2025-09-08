#if ENABLE_DEBUG
using UnityEditor;

namespace ST.GmTools
{
    [ET.EnableClass]
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
        
        private DrawViewGroup mView;
        private void Awake()
        {
            mView = GenGmToolsView();
        }

        private void OnGUI()
        {
            // 代码编译完成后局部变量被清空
            if (mView == null)
                mView = GenGmToolsView();
            
            mView.OnDraw();
        }
        
        private DrawViewGroup GenGmToolsView()
        {
            DrawViewGroup drawViewGroup = new DrawViewGroup();
            drawViewGroup.RegisterDrawView("Battle/Global", new DrawViewBattle());
            drawViewGroup.SelectDrawView("Battle/Global");
            return drawViewGroup;
        }
    }
}
#endif