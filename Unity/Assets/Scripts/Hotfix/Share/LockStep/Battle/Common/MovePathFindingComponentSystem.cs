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
        private static void Awake(this MovePathFindingComponent self, bool isUseRVO)
        {
            self.IsUseRVO = isUseRVO;
            self.PathPoints = new List<TSVector>();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this MovePathFindingComponent self)
        {
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
                if (self.IsUseRVO) {
                    transformComponent.RVOMove(new TSVector2(targetDir.x, targetDir.z));
                } else {
                    transformComponent.Move(new TSVector2(targetDir.x, targetDir.z));
                }
            }
        }
        
        // isCanInterrupt 是否可以被打断，如果不可以被打断，则在移动过程中不会进入AI警戒状态
        public static void PathFinding(this MovePathFindingComponent self, TSVector2 position, bool isCanInterrupt)
        {
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            
            TSVector pos = new TSVector(position.x, transformComponent.Position.y, position.y);
            if (gridMapComponent.Pathfinding(transformComponent.Position, pos, self.PathPoints))
            {
                // 成功找到路径，终点位置修正为目标位置
                self.PathPoints[^1] = gridMapComponent.ClampPosition(pos);
                if (!isCanInterrupt && !self.IsRefrenceNotAIAlert) {
                    self.IsRefrenceNotAIAlert = true;
                    self.LSOwner().GetComponent<FlagComponent>().AddRestrict((int)FlagRestrict.NotAIAlert);
                }
            }

            // 忽略起点位置，使用当前位置为起点
            self.CurrentPathIndex = 1;
        }
        
        public static void Stop(this MovePathFindingComponent self)
        {
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