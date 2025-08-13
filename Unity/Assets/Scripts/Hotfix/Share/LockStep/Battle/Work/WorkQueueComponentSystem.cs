
namespace ET
{
    [LSEntitySystemOf(typeof(WorkQueueComponent))]
    [EntitySystemOf(typeof(WorkQueueComponent))]
    [FriendOf(typeof(WorkQueueComponent))]
    public static partial class WorkQueueComponentSystem
    {
        [EntitySystem]
        private static void Awake(this WorkQueueComponent self)
        {
            
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this WorkQueueComponent self)
        {
            PropComponent propComponent = self.LSOwner().GetComponent<PropComponent>();
            var workerCount = propComponent.Get(NumericType.Population).AsInt();
            var freeCount = workerCount - self.WorkerCount;
            
            int indexBegin = 0;
            int indexRight = self.WorkComponents.Count - 1;
            for (; indexRight >= indexBegin; indexRight--)
            {
                WorkComponent workComponent = self.WorkComponents[indexRight];
                if (workComponent.WorkerCount == workComponent.WorkerLimit)
                    continue;
                
                if (workComponent.LSOwner().EnabledRef < 0) {
                    freeCount += workComponent.WorkerCount;
                    workComponent.WorkerCount = 0;
                    continue;
                }
                
                // 若闲暇工人足够 则直接分配
                int needCount = workComponent.WorkerLimit - workComponent.WorkerCount;
                if (freeCount >= needCount) {
                    freeCount -= needCount;
                    workComponent.WorkerCount += needCount;
                    continue;
                }
                
                // 把闲暇的工人先行分配
                if (freeCount > 0)
                {
                    needCount -= freeCount;
                    workComponent.WorkerCount += freeCount;
                    freeCount = 0;
                }

                // 剩余的岗位从前往后抢夺（后添加的约定为优先级最高）
                int count = self.SnatchingWorkers(needCount, ref indexBegin, indexRight);
                if (count > 0) {
                    workComponent.WorkerCount += count;
                }
            }
            self.WorkerCount = workerCount - freeCount;
        }

        private static int SnatchingWorkers(this WorkQueueComponent self, int need, ref int indexBegin, int indexEnd)
        {
            var count = 0;
            for (; indexBegin < indexEnd; indexBegin++)
            {
                WorkComponent workComponent = self.WorkComponents[indexBegin];
                if (workComponent.WorkerCount == 0)
                    continue;
                
                if (need > workComponent.WorkerCount) {
                    need -= workComponent.WorkerCount;
                    count += workComponent.WorkerCount;
                    workComponent.WorkerCount = 0;
                    continue;
                }

                workComponent.WorkerCount -= need;
                count += need;
                break;
            }
            return count;
        }

    }
}