#if ENABLE_DEBUG
using UnityEngine;

namespace ST.GmTools
{
    public abstract class DrawViewScrollable : IDrawView
    {
        private Vector2 mScrollPosition = Vector2.zero;
        
        protected abstract void OnDrawScrollableView();
        
        public void OnDraw()
        {
            mScrollPosition = GUILayout.BeginScrollView(mScrollPosition);
            {
                OnDrawScrollableView();
            }
            GUILayout.EndScrollView();
        }
    }
}
#endif