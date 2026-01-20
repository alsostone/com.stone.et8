using UnityEditor;
using UnityEngine;

namespace ST.Mono
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DynamicTreeBehaviour))]
    public class DynamicTreeBehaviourEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DynamicTreeBehaviour mono = (DynamicTreeBehaviour)target;
            if (GUILayout.Button("Spawn NodeObject")) {
                mono.SpawnNodeObject();
            }
            if (GUILayout.Button("Rebuild BottomUp")) {
                mono.RebuildBottomUp();
            }
        }
    }
}