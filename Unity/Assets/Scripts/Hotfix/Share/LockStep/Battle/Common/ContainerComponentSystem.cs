using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(ContainerComponent))]
    [FriendOf(typeof(ContainerComponent))]
    public static partial class ContainerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ContainerComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(185, self.LSParent().Id);
            self.Contents = new List<long>();
        }
        
        public static void AddContent(this ContainerComponent self, long instanceId)
        {self.LSRoom()?.ProcessLog.LogFunction(184, self.LSParent().Id);
            self.Contents.Add(instanceId);
        }
        
        public static void RemoveContent(this ContainerComponent self, long instanceId)
        {self.LSRoom()?.ProcessLog.LogFunction(183, self.LSParent().Id);
            self.Contents.Remove(instanceId);
        }
        
        public static long GetFirstContent(this ContainerComponent self)
        {
            if (self.Contents.Count == 0)
            {
                return 0;
            }
            return self.Contents[0];
        }
    }
    
}
