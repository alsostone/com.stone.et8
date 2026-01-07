using UnityEditor;
using UnityEngine;

namespace ST.Mono
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(NaturalSpawnSampler))]
    public class NaturalSpawnSamplerEditor : Editor
    {
        private NaturalSpawnSampler sampler;

        private void OnEnable()
        {
            sampler = (NaturalSpawnSampler)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("固定生成"))
            {
                sampler.Generate();
            }
            
            if (GUILayout.Button("随机生成"))
            {
                sampler.GenerateRandom();
            }

            if (GUILayout.Button("清除数据"))
            {
                sampler.ClearAllData();
            }
        }
    }
}