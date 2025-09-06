#if ENABLE_DEBUG
using System;
using System.Collections.Generic;

namespace ST.GmTools
{
    public sealed class DrawViewGroup : IDrawViewGroup, IDrawView
    {
        private readonly List<KeyValuePair<string, IDrawView>> mDrawViews;
        private int mSelectedIndex;
        private string[] mDrawViewNames;

        public DrawViewGroup()
        {
            mDrawViews = new List<KeyValuePair<string, IDrawView>>();
            mSelectedIndex = 0;
            mDrawViewNames = null;
        }

        public int DrawViewCount => mDrawViews.Count;

        public int SelectedIndex
        {
            get => mSelectedIndex;
            set => mSelectedIndex = value;
        }

        public IDrawView SelectedView => mSelectedIndex >= mDrawViews.Count ? null : mDrawViews[mSelectedIndex].Value;

        public void OnDraw() { }

        private void ResetViewNames()
        {
            var num = 0;
            mDrawViewNames = new string[mDrawViews.Count];
            foreach (var debuggerWindow in mDrawViews)
                mDrawViewNames[num++] = debuggerWindow.Key;
        }

        public string[] GetDrawViewNames() => mDrawViewNames;

        public IDrawView GetDrawView(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            var length = path.IndexOf('/');
            if (length < 0 || length >= path.Length - 1)
                return GetDrawViewInternal(path);
            var name = path.Substring(0, length);
            var sub = path.Substring(length + 1);
            return ((DrawViewGroup) GetDrawViewInternal(name))?.GetDrawView(sub);
        }

        public bool SelectDrawView(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            var length = path.IndexOf('/');
            if (length < 0 || length >= path.Length - 1)
                return SelectDrawViewInternal(path);
            var name = path.Substring(0, length);
            var path1 = path.Substring(length + 1);
            var debuggerWindow = (DrawViewGroup) GetDrawViewInternal(name);
            return debuggerWindow != null && SelectDrawViewInternal(name) && debuggerWindow.SelectDrawView(path1);
        }

        public void RegisterDrawView(string path, IDrawView view)
        {
            var length = !string.IsNullOrEmpty(path) ? path.IndexOf('/') : throw new Exception("path is invalid.");
            if (length < 0 || length >= path.Length - 1) {
                if (GetDrawViewInternal(path) != null)
                    throw new Exception("view has been registered.");
                mDrawViews.Add(new KeyValuePair<string, IDrawView>(path, view));
                ResetViewNames();
            }
            else {
                var str = path.Substring(0, length);
                var sub = path.Substring(length + 1);
                var viewGroup = (DrawViewGroup) GetDrawViewInternal(str);
                if (viewGroup == null) {
                    if (GetDrawViewInternal(str) != null)
                        throw new Exception("view has been registered, can not create debugger window group.");
                    viewGroup = new DrawViewGroup();
                    mDrawViews.Add(new KeyValuePair<string, IDrawView>(str, viewGroup));
                    ResetViewNames();
                }

                viewGroup.RegisterDrawView(sub, view);
            }
        }

        public bool UnregisterDrawView(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            var length = path.IndexOf('/');
            if (length < 0 || length >= path.Length - 1) {
                var debuggerWindow = GetDrawViewInternal(path);
                var num = mDrawViews.Remove(new KeyValuePair<string, IDrawView>(path, debuggerWindow)) ? 1 : 0;
                ResetViewNames();
                return num != 0;
            }

            var name = path.Substring(0, length);
            var path1 = path.Substring(length + 1);
            var debuggerWindow1 = (DrawViewGroup) GetDrawViewInternal(name);
            return debuggerWindow1 != null && debuggerWindow1.UnregisterDrawView(path1);
        }

        private IDrawView GetDrawViewInternal(string name)
        {
            foreach (var pair in mDrawViews) {
                if (pair.Key == name)
                    return pair.Value;
            }
            return null;
        }

        private bool SelectDrawViewInternal(string name)
        {
            for (var index = 0; index < mDrawViews.Count; ++index) {
                if (mDrawViews[index].Key == name) {
                    mSelectedIndex = index;
                    return true;
                }
            }

            return false;
        }
    }
}
#endif