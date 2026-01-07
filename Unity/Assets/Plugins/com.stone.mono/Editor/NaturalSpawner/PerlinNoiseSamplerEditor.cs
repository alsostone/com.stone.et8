using UnityEditor;
using UnityEngine;

namespace ST.Mono
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PerlinNoiseSampler))]
    public class PerlinNoiseSamplerEditor : Editor
    {
        private PerlinNoiseSampler sampler;

        private void OnEnable()
        {
            sampler = (PerlinNoiseSampler)target;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            
            if (GUILayout.Button("生成固定噪声"))
            {
                sampler.GenerateNoise();
            }
            
            if (GUILayout.Button("生成随机噪声"))
            {
                sampler.GenerateNoise(Random.Range(int.MinValue, int.MaxValue), sampler.RegionSize);
            }

            if (GUILayout.Button("清除噪声"))
            {
                sampler.ClearNoise();
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("导出噪声值"))
            {
                SaveNoiseValues(sampler.NoiseValues);
            }
            
            if (GUILayout.Button("导出纹理"))
            {
                SaveTexture(sampler.NoiseTexture);
            }
        }
        
        private void SaveNoiseValues(float[,] noiseValues)
        {
            if (noiseValues == null)
            {
                EditorUtility.DisplayDialog("错误", "没有可保存的噪声值", "确定");
                return;
            }

            string path = EditorUtility.SaveFilePanel("导出噪声值", "Assets/", "PerlinNoise", "txt");
            if (!string.IsNullOrEmpty(path))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                int width = noiseValues.GetLength(0);
                int height = noiseValues.GetLength(1);
                
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        sb.Append(noiseValues[x, y].ToString("F4"));
                        if (x < width - 1)
                            sb.Append(", ");
                    }
                    sb.AppendLine();
                }

                System.IO.File.WriteAllText(path, sb.ToString());
                
                // 刷新资源数据库
                AssetDatabase.Refresh();
                
                Debug.Log($"噪声值已保存到: {path}");
            }
        }
        
        private void SaveTexture(Texture2D texture)
        {
            if (texture == null)
            {
                EditorUtility.DisplayDialog("错误", "没有可保存的纹理", "确定");
                return;
            }

            string path = EditorUtility.SaveFilePanel("导出纹理", "Assets/", "PerlinNoise", "png");
            if (!string.IsNullOrEmpty(path))
            {
                byte[] bytes = texture.EncodeToPNG();
                System.IO.File.WriteAllBytes(path, bytes);
                
                // 刷新资源数据库
                AssetDatabase.Refresh();
                
                Debug.Log($"纹理已保存到: {path}");
            }
        }
    }
}