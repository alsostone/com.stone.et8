using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ST.Mono
{
    [CustomEditor(typeof(PoissonDiskSampler))]
    public class PoissonDiskSamplerEditor : Editor
    {
        private PoissonDiskSampler sampler;

        private void OnEnable()
        {
            sampler = (PoissonDiskSampler)target;
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("生成固定点集"))
            {
                sampler.GeneratePoints();
                SceneView.RepaintAll();
            }
            
            if (GUILayout.Button("生成随机点集"))
            {
                sampler.GeneratePoints(Random.Range(int.MinValue, int.MaxValue), sampler.MinDistance, sampler.RegionSize);
                SceneView.RepaintAll();
            }
            
            if (GUILayout.Button("清除点集"))
            {
                sampler.ClearPoints();
                SceneView.RepaintAll();
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("获取统计信息"))
            {
                string stats = sampler.GetStats();
                Debug.Log($"泊松圆盘采样统计:\n{stats}");
            }
            
            if (GUILayout.Button("导出点数据"))
            {
                ExportPointData();
            }
        }
        
        private void ExportPointData()
        {
            if (sampler.PointCount == 0)
            {
                EditorUtility.DisplayDialog("错误", "没有点数据可导出", "确定");
                return;
            }
            
            string path = EditorUtility.SaveFilePanel("导出点数据", "Assets/", "PoissonPoints", "txt");
            if (!string.IsNullOrEmpty(path))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine("泊松圆盘采样点数据");
                sb.AppendLine($"点数: {sampler.PointCount}");
                sb.AppendLine($"区域大小: {sampler.RegionSize}");
                sb.AppendLine($"最小距离: {sampler.MinDistance}");
                sb.AppendLine();
                sb.AppendLine("点坐标 (X, Y):");
                
                List<Vector3> positions = sampler.GetWorldPositions();
                foreach (Vector3 pos in positions)
                {
                    sb.AppendLine($"{pos.x:F3}, {pos.z:F3}");
                }
                
                System.IO.File.WriteAllText(path, sb.ToString());
                Debug.Log($"点数据已导出到: {path}");
            }
        }
    }
}