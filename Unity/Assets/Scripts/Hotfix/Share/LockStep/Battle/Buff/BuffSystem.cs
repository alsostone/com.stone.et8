namespace ET
{
    [LSEntitySystemOf(typeof(Buff))]
    [EntitySystemOf(typeof(Buff))]
    [FriendOf(typeof(Buff))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this Buff self, int BuffId, LSUnit caster)
        {self.LSRoom().ProcessLog.LogFunction(16, self.LSParent().Id, BuffId, caster.Id);
            self.BuffId = BuffId;
            self.Caster = caster.Id;
            self.StartFrame = self.LSWorld().Frame;
            self.EndFrame = self.StartFrame + self.TbBuffRow.Duration.Convert2Frame();
            self.LayerCount = 1;
            if (self.TbBuffRow.EnterEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.EnterEffect, caster, self.LSOwner());
            }
            if (self.TbBuffRow.IntervalEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.IntervalEffect, self.LSUnit(self.Caster), self.LSOwner());
                self.IntervalFrame = self.StartFrame + self.TbBuffRow.Interval.Convert2Frame();
            }
        }

        [EntitySystem]
        private static void Destroy(this Buff self)
        {self.LSRoom().ProcessLog.LogFunction(15, self.LSParent().Id);
            if (self.TbBuffRow.EnterEffect > 0) {
                EffectExecutor.ReverseExecute(self.TbBuffRow.EnterEffect, self.LSUnit(self.Caster), self.LSOwner());
            }
            if (self.TbBuffRow.FinishEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.FinishEffect, self.LSUnit(self.Caster), self.LSOwner());
            }
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this Buff self)
        {self.LSRoom().ProcessLog.LogFunction(14, self.LSParent().Id);
            if (self.LSWorld().Frame > self.EndFrame)
            {
                self.GetParent<BuffComponent>().RemoveBuff(self.BuffId, true);
                return;
            }

            if (self.TbBuffRow.IntervalEffect > 0 && self.LSWorld().Frame > self.IntervalFrame)
            {
                EffectExecutor.Execute(self.TbBuffRow.IntervalEffect, self.LSUnit(self.Caster), self.LSOwner());
                self.IntervalFrame = self.LSWorld().Frame + self.TbBuffRow.Interval.Convert2Frame();
            }
        }

        public static void ResetEndFrame(this Buff self)
        {self.LSRoom().ProcessLog.LogFunction(13, self.LSParent().Id);
            self.EndFrame = self.LSWorld().Frame + self.TbBuffRow.Duration.Convert2Frame();
        }
    }
}