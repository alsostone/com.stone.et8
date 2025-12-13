namespace ET
{
    [EntitySystemOf(typeof(Buff))]
    [FriendOf(typeof(Buff))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this Buff self, int BuffId, LSUnit caster)
        {self.LSRoom()?.ProcessLog.LogFunction(41, self.LSParent().Id, BuffId, caster.Id);
            self.BuffId = BuffId;
            self.Caster = caster.Id;
            self.StartFrame = self.LSWorld().Frame;
            self.EndFrame = self.StartFrame + self.TbBuffRow.Duration.Convert2Frame();
            self.LayerCount = 1;
            
            LSUnit lsOwner = self.LSOwner();
            if (self.TbBuffRow.EnterEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.EnterEffect, caster, lsOwner, lsOwner, self.LayerCount);
            }
            if (self.TbBuffRow.IntervalEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.IntervalEffect, caster, lsOwner, lsOwner, self.LayerCount);
                self.IntervalFrame = self.StartFrame + self.TbBuffRow.Interval.Convert2Frame();
            }
        }

        [EntitySystem]
        private static void Destroy(this Buff self)
        {self.LSRoom()?.ProcessLog.LogFunction(40, self.LSParent().Id);
            LSUnit lsOwner = self.LSOwner();
            LSUnit lsCaster = self.LSUnit(self.Caster);
            if (self.TbBuffRow.EnterEffect > 0) {
                EffectExecutor.ReverseExecute(self.TbBuffRow.EnterEffect, lsCaster, lsOwner, self.LayerCount);
            }
            if (self.TbBuffRow.FinishEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.FinishEffect, lsCaster, lsOwner, lsOwner, self.LayerCount);
            }
        }
        
        public static void ResetEndFrame(this Buff self)
        {self.LSRoom()?.ProcessLog.LogFunction(39, self.LSParent().Id);
            self.EndFrame = self.LSWorld().Frame + self.TbBuffRow.Duration.Convert2Frame();
        }

        public static void TryExecuteInterval(this Buff self)
        {self.LSRoom()?.ProcessLog.LogFunction(38, self.LSParent().Id);
            if (self.TbBuffRow.IntervalEffect > 0 && self.LSWorld().Frame >= self.IntervalFrame) {
                LSUnit lsOwner = self.LSOwner();
                LSUnit lsCaster = self.LSUnit(self.Caster);
                EffectExecutor.Execute(self.TbBuffRow.IntervalEffect, lsCaster, lsOwner, lsOwner, self.LayerCount);
                self.IntervalFrame = self.LSWorld().Frame + self.TbBuffRow.Interval.Convert2Frame();
            }
        }

        public static void IncrLayerCount(this Buff self)
        {self.LSRoom()?.ProcessLog.LogFunction(165, self.LSParent().Id);
            self.LayerCount++;
            if (self.TbBuffRow.EnterEffect > 0) {
                LSUnit lsOwner = self.LSOwner();
                LSUnit lsCaster = self.LSUnit(self.Caster);
                EffectExecutor.Execute(self.TbBuffRow.EnterEffect, lsCaster, lsOwner, lsOwner);
            }
        }
        
        public static int DecrLayerCount(this Buff self)
        {self.LSRoom()?.ProcessLog.LogFunction(164, self.LSParent().Id);
            self.LayerCount--;
            if (self.TbBuffRow.EnterEffect > 0 && self.LayerCount >= 0) {
                LSUnit lsOwner = self.LSOwner();
                LSUnit lsCaster = self.LSUnit(self.Caster);
                EffectExecutor.ReverseExecute(self.TbBuffRow.EnterEffect, lsCaster, lsOwner);
            }

            return self.LayerCount;
        }
    }
}