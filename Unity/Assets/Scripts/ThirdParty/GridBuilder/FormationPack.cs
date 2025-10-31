using System.Collections.Generic;
using TrueSync;

namespace ST.GridBuilder
{
    public static class FormationPack
    {
        // 复用列表避免GC
        private static readonly List<TSVector> formationPositions = new (32);
        private static readonly List<TSVector2> initialLocal = new (32);
        private static readonly List<TSVector2> targetLocal = new (32);
        private static readonly List<int> initialIndices = new (32);
        private static readonly List<int> targetIndices = new (32);

        public static void FormationGridPack(List<TSVector> positions, TSVector center, TSVector destination, FP radius, TSVector forward, TSVector? up = null)
        {
            TSVector right = TSVector.Cross(up ?? TSVector.up, forward).normalized;
            
            // 目标为接近长宽比3:2的矩形
            int columns = TSMath.Ceiling(TSMath.Sqrt(positions.Count) * FP.Ratio(3, 2)).AsInt();
            int rows = TSMath.Ceiling((FP)positions.Count / columns).AsInt();
        
            FP spacing = radius * 2;
            FP width = (columns - 1) * spacing;
            FP depth = (rows - 1) * spacing;

            // 计算Grid阵型位置
            formationPositions.Clear();
            TSVector start = destination - right * (width * FP.Half) + forward * (depth * FP.Half);
            for (int i = 0; i < positions.Count; i++) {
                int row = i / columns;
                int col = i % columns;
                TSVector position = start + right * (col * spacing) - forward * (row * spacing);
                formationPositions.Add(position);
            }
            
            RelativePositionAssignment(positions, center, formationPositions, destination, forward, right);
        }
        
        /// <summary>
        /// 基于相对位置分配（保持队形关系）
        /// </summary>
        private static void RelativePositionAssignment(List<TSVector> initialPositions, TSVector initialCenter, 
                List<TSVector> targetPositions, TSVector targetCenter, TSVector forward, TSVector right)
        {
            // 将初始位置转换到以中心为原点的局部坐标系
            initialLocal.Clear();
            initialIndices.Clear();
            for (int index = 0; index < initialPositions.Count; index++)
            {
                TSVector pos = initialPositions[index];
                TSVector relative = pos - initialCenter;
                FP x = TSVector.Dot(relative, right);
                FP y = TSVector.Dot(relative, forward);
                initialLocal.Add(new TSVector2(x, y));
                initialIndices.Add(index);
            }

            // 将目标位置转换到局部坐标系
            targetLocal.Clear();
            targetIndices.Clear();
            for (int index = 0; index < targetPositions.Count; index++)
            {
                TSVector pos = targetPositions[index];
                TSVector relative = pos - targetCenter;
                FP x = TSVector.Dot(relative, right);
                FP y = TSVector.Dot(relative, forward);
                targetLocal.Add(new TSVector2(x, y));
                targetIndices.Add(index);
            }
            
            SortByAngleAndDistance(initialIndices, initialLocal);
            SortByAngleAndDistance(targetIndices, targetLocal);

            for (int i = 0; i < initialPositions.Count; i++) {
                initialPositions[initialIndices[i]] = targetPositions[targetIndices[i]];
            }
        }
        
        private static void SortByAngleAndDistance(List<int> indices, List<TSVector2> localPositions)
        {
            indices.Sort((a, b) =>
            {
                TSVector2 posA = localPositions[a];
                TSVector2 posB = localPositions[b];

                int angleComparison = TSMath.Atan2(posA.y, posA.x).CompareTo(TSMath.Atan2(posB.y, posB.x));
                if (angleComparison != 0)
                    return angleComparison; // 按角度排序

                return posA.sqrMagnitude.CompareTo(posB.sqrMagnitude); // 按距离排序
            });
        }
    }
}