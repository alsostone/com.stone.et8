#if ENABLE_DEBUG
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ST.GmTools
{
    [ET.EnableClass]
    public class GmToolsBehaviour : MonoBehaviour
    {
        private float mWindowScale = 0;
        [ET.StaticField] private static readonly Vector2 WindowSize = new Vector2(820f, 460f);
        private readonly Rect mDragRect = new Rect(0f, 0f, float.MaxValue, 20f);
        private Rect mWindowRect = new Rect();
        private GmToolsView mView;
        private EventSystem mCacheEventSystem;

        private void OnEnable()
        {
            if (mView == null)
                mView = new GmToolsView(() => { enabled = false; });
            mCacheEventSystem = EventSystem.current;
            mCacheEventSystem.enabled = false;
        }

        private void OnDisable()
        {
            if (mCacheEventSystem)
                mCacheEventSystem.enabled = true;
        }

        private void OnGUI()
        {
            var scale = Mathf.Min(Screen.width / WindowSize.x * 0.9f, Screen.height / WindowSize.y * 0.9f);
            if (Math.Abs(scale - mWindowScale) > 0.1f) {
                mWindowScale = scale;
                mWindowRect = new Rect(((float)Screen.width / mWindowScale - WindowSize.x) / 2, ((float)Screen.height / mWindowScale - WindowSize.y) / 2, WindowSize.x,WindowSize.y);
            }
            
            var cachedMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.Scale(new Vector3(mWindowScale, mWindowScale, 1f));
            mWindowRect = GUILayout.Window(2, mWindowRect, DrawWindow, "<b>GmTools</b>");
            GUI.matrix = cachedMatrix;
        }

        private void DrawWindow(int windowId)
        {
            // 需求不可拖拽window防止乱拖后回不来
            // GUI.DragWindow(mDragRect);
            mView.OnGUI();
        }
    }
    
}
#endif