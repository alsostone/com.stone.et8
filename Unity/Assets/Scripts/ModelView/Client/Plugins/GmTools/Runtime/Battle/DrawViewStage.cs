#if ENABLE_DEBUG
using UnityEngine;

namespace ST.GmTools
{
    [ET.EnableClass]
    public sealed class DrawViewStage : DrawViewScrollable
    {
        protected override void OnDrawScrollableView()
        {
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
        }
    }
}
#endif