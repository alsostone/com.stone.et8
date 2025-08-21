namespace ET
{
    [LSEntitySystemOf(typeof(WorkComponent))]
    [EntitySystemOf(typeof(WorkComponent))]
    [FriendOf(typeof(WorkComponent))]
    public static partial class WorkComponentSystem
    {
        [EntitySystem]
        private static void Awake(this WorkComponent self, int workerLimit)
        {self.LSRoom()?.ProcessLog.LogFunction(110, self.LSParent().Id, workerLimit);
            self.WorkerLimit = workerLimit;
            self.AddToQueue();
        }
        
        [LSEntitySystem]
        private static void Deserialize(this WorkComponent self)
        {
            self.AddToQueue();
        }
        
        private static void AddToQueue(this WorkComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(109, self.LSParent().Id);
            TeamType team = self.LSOwner().GetComponent<TeamComponent>().Type;
            WorkQueueComponent component = self.LSTeamUnit(team).GetComponent<WorkQueueComponent>();
            component.WorkComponents.Add(self);
        }
        
    }
}