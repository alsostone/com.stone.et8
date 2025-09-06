#if ENABLE_DEBUG
using UnityEngine;

namespace ST.GmTools
{
    public sealed class DrawViewBattle : DrawViewScrollable
    {
        public DrawViewBattle()
        {
        }

        protected override void OnDrawScrollableView()
        {
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
        }
        
    }
}
#endif