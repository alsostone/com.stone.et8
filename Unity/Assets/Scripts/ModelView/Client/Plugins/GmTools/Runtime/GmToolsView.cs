#if ENABLE_DEBUG
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ST.GmTools
{
    [ET.EnableClass]
    public class GmToolsView
    {
        private readonly DrawViewMgr mDrawViewMgr;
        [ET.StaticField] private static int sGroupNumber;
        [ET.StaticField] private static readonly Dictionary<int, int> SelectGroupIndex = new Dictionary<int, int>();
        private readonly Action mOnCloseCallback; 

        public GmToolsView(Action closeCallback)
        {
            mOnCloseCallback = closeCallback;
            mDrawViewMgr = new DrawViewMgr();
            mDrawViewMgr.RegisterDrawView("Battle/Global", new DrawViewBattle());
        }

        public void OnGUI()
        {
            if (mDrawViewMgr != null) {
                sGroupNumber = 0;
                OnDrawViewGroup(mDrawViewMgr.ViewRoot, true);
            }
        }
        
        private void OnDrawViewGroup(IDrawViewGroup viewGroup, bool isRoot)
        {
            if (viewGroup == null)
                return;

            var names = new List<string>();
            var viewNames = viewGroup.GetDrawViewNames();
            foreach (var t in viewNames) {
                names.Add(t);
            }

            var groupNumber = 0;
            for (var i = 0; i <= sGroupNumber; i++) {
                groupNumber += i * (1 << 5 * (6 - i));      // 每层菜单最多占5位（32个） 且最多6层
            }
            SelectGroupIndex.TryGetValue(groupNumber, out var index);
            if (index >= names.Count)
                index = names.Count - 1;
            
            GUILayout.BeginHorizontal();
            index = GUILayout.Toolbar(index, names.ToArray(), GUI.skin.button , GUILayout.Width(80f * names.Count));
            GUILayout.FlexibleSpace();
            if (isRoot && mOnCloseCallback != null && GUILayout.Button("关闭", GUILayout.Width(60))) {
                mOnCloseCallback();
            }
            GUILayout.EndHorizontal();
            viewGroup.SelectedIndex = index;
            SelectGroupIndex[groupNumber] = index;
            sGroupNumber++;
            
            if (viewGroup.SelectedView is IDrawViewGroup subViewGroup) {
                OnDrawViewGroup(subViewGroup, false);
            }

            viewGroup.SelectedView.OnDraw();
        }

        public void Close()
        {
            mOnCloseCallback?.Invoke();
        }
    }
}
#endif