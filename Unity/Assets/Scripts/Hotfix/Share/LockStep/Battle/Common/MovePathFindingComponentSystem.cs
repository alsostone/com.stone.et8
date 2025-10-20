using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(MovePathFindingComponent))]
    [EntitySystemOf(typeof(MovePathFindingComponent))]
    [FriendOf(typeof(MovePathFindingComponent))]
    public static partial class MovePathFindingComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MovePathFindingComponent self)
        {
            self.PathPoints = new List<TSVector>();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this MovePathFindingComponent self)
        {
            if (self.PathPoints.Count > self.CurrentPathIndex)
            {
                TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
                TSVector dir = self.PathPoints[self.CurrentPathIndex] - transformComponent.Position;
                transformComponent.Move(new TSVector2(dir.x, dir.z));

                TSVector dir2 = self.PathPoints[self.CurrentPathIndex] - transformComponent.Position;
                if (dir2.sqrMagnitude < FP.EN4 || TSVector.Dot(dir, dir2) <= 0) {
                    self.CurrentPathIndex++;
                }
            }
        }
        
        public static void PathFinding(this MovePathFindingComponent self, TSVector2 position)
        {
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            
            TSVector pos = new TSVector(position.x, transformComponent.Position.y, position.y);
            if (gridMapComponent.Pathfinding(transformComponent.Position, pos, self.PathPoints))
            {
                // 成功找到路径，终点位置修正为目标位置
                self.PathPoints[^1] = pos;
            }

            // 忽略起点位置，使用当前位置为起点
            self.CurrentPathIndex = 1;
        }
        
        public static void Stop(this MovePathFindingComponent self)
        {
            self.PathPoints.Clear();
            self.CurrentPathIndex = 0;
        }
    }
}