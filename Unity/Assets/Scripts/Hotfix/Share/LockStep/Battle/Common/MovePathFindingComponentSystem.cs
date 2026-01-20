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
        {self.LSRoom()?.ProcessLog.LogFunction(77, self.LSParent().Id);
            self.PathPoints = new List<TSVector>();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this MovePathFindingComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(76, self.LSParent().Id);
            if (self.PathPoints.Count > self.CurrentPathIndex)
            {
                TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
                TSVector targetDir = self.PathPoints[self.CurrentPathIndex] - self.PathPoints[self.CurrentPathIndex - 1];
                TSVector currentDir = self.PathPoints[self.CurrentPathIndex] - transformComponent.Position;
                if (currentDir.sqrMagnitude < FP.EN4 || TSVector.Dot(targetDir, currentDir) <= 0)
                {
                    self.CurrentPathIndex++;
                    if (self.CurrentPathIndex >= self.PathPoints.Count) {
                        self.Stop();
                    }
                }
                
                // 放在后面是因为RVO移动不能立即改变位置，需要等下一帧才能获取到最新位置
                transformComponent.RVOMove(new TSVector2(targetDir.x, targetDir.z));
            }
        }
        
        public static void SetDestination(this MovePathFindingComponent self, TSVector position, MovementMode movementMode)
        {self.LSRoom()?.ProcessLog.LogFunction(75, self.LSParent().Id, position.x.V, position.y.V, position.z.V);
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            
            if (gridMapComponent.Pathfinding(transformComponent.Position, position, self.PathPoints))
            {
                // 成功找到路径，终点位置修正为目标位置
                self.PathPoints[^1] = gridMapComponent.ClampPosition(position);
                if (movementMode == MovementMode.Move && !self.IsRefrenceNotAIAlert) {
                    self.IsRefrenceNotAIAlert = true;
                    self.LSOwner().GetComponent<FlagComponent>().AddRestrict((int)FlagRestrict.NotAIAlert);
                }
            }

            // 忽略起点位置，使用当前位置为起点
            self.CurrentPathIndex = 1;
        }
        
        public static void Stop(this MovePathFindingComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(74, self.LSParent().Id);
            if (self.PathPoints.Count > 0) {
                self.PathPoints.Clear();
                self.CurrentPathIndex = 0;
            }
            if (self.IsRefrenceNotAIAlert) {
                self.LSOwner().GetComponent<FlagComponent>().RemoveRestrict((int)FlagRestrict.NotAIAlert);
                self.IsRefrenceNotAIAlert = false;
            }
        }
    }
}