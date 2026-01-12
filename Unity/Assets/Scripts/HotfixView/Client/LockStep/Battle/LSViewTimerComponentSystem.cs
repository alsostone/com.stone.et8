namespace ET.Client
{
    [LSEntitySystemOf(typeof(LSViewTimerComponent))]
    [EntitySystemOf(typeof(LSViewTimerComponent))]
    [FriendOf(typeof(LSViewTimerComponent))]
    public static partial class LSViewTimerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewTimerComponent self)
        {
            self.TimerSpeed = self.Room().TimeScale;
            self.PrevFrameTime = TimeInfo.Instance.ServerFrameTime();
        }

        [LSEntitySystem]
        private static void LSRollback(this LSViewTimerComponent self)
        {
            self.TimerSpeed = self.Room().TimeScale;
        }

        [EntitySystem]
        private static void Update(this LSViewTimerComponent self)
        {
            long frameTime = TimeInfo.Instance.ServerFrameTime();
            float deltaTime = (frameTime - self.PrevFrameTime) / 1000f * self.TimerSpeed;
            
            while (self.RemoveTimers.Count > 0)
            {
                LSViewTimer lsViewTimer = self.RemoveTimers.Dequeue();
                lsViewTimer.Dispose();
            }

            foreach (Entity child in self.Children.Values)
            {
                LSViewTimer lsViewTimer = (LSViewTimer)child;
                int result = lsViewTimer.Tick(deltaTime);
                switch (result)
                {
                    case 1:
                        self.TimeoutTimers.Enqueue(lsViewTimer);
                        break;
                    case 0:
                        self.TimeoutTimers.Enqueue(lsViewTimer);
                        self.RemoveTimers.Enqueue(lsViewTimer);
                        break;
                }
            }
            
            while (self.TimeoutTimers.Count > 0)
            {
                LSViewTimer lsViewTimer = self.TimeoutTimers.Dequeue();
                EventSystem.Instance.Invoke(lsViewTimer.Type, new TimerCallback() { Args = lsViewTimer.Object });
            }
            while (self.RemoveTimers.Count > 0)
            {
                LSViewTimer lsViewTimer = self.RemoveTimers.Dequeue();
                lsViewTimer.Dispose();
            }
            self.PrevFrameTime = frameTime;
        }
        
        public static void SetSpeed(this LSViewTimerComponent self, float speed)
        {
            self.TimerSpeed = speed;
        }
        
        public static long AddTimer(this LSViewTimerComponent self, long millisecond, int type, object args, int loop = 0)
        {
            return self.AddChild<LSViewTimer, float, int, object, int>(millisecond / 1000f, type, args, loop).Id;
        }
        
        public static long AddTimer(this LSViewTimerComponent self, float second, int type, object args, int loop = 0)
        {
            return self.AddChild<LSViewTimer, float, int, object, int>(second, type, args, loop).Id;
        }
        
        public static void RemoveTimer(this LSViewTimerComponent self, ref long id)
        {
            if (id == 0) return;
            
            LSViewTimer lsViewTimer = self.GetChild<LSViewTimer>(id);
            if (lsViewTimer != null)
                self.RemoveTimers.Enqueue(lsViewTimer);
            id = 0;
        }
    }
    
    [EntitySystemOf(typeof(LSViewTimer))]
    [FriendOf(typeof(LSViewTimer))]
    public static partial class LSViewTimerSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewTimer self, float time, int type, object obj, int loop)
        {
            self.ElapseTime = 0;
            self.Time = time;
            self.Type = type;
            self.Object = obj;
            self.Loop = loop;
        }
        
        public static int Tick(this LSViewTimer self, float deltaTime)
        {
            self.ElapseTime += deltaTime;
            if (self.ElapseTime >= self.Time)
            {
                if (self.Loop > 0) {
                    self.Loop--;
                }
                if (self.Loop != 0) {
                    self.ElapseTime -= self.Time;
                    return 1;
                }
                return 0;
            }
            return -1;
        }
        
    }

}