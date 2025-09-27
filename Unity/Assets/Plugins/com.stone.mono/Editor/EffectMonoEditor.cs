using UnityEditor;
using UnityEngine;

namespace ST.Mono
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EffectMono))]
    public class EffectMonoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EffectMono mono = (EffectMono)target;
            if (GUILayout.Button("播放（Play模式）")) {
                mono.Play();
            }
            if (GUILayout.Button("停止")) {
                mono.Stop();
            }
        }
    }
}