namespace ET
{
    [LSEntitySystemOf(typeof(Buff))]
    [EntitySystemOf(typeof(Buff))]
    [FriendOf(typeof(Buff))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this Buff self, int BuffId)
        {
            self.BuffId = BuffId;
            self.StartTime = TimeInfo.Instance.ServerNow();
            self.IntervalTime = self.StartTime + self.TbBuffRow.Interval;
            self.LayerCount = 1;
            
            LSUnitComponent unitComponent = self.Owner.GetParent<LSWorld>().GetComponent<LSUnitComponent>();
            LSUnit caster = unitComponent.GetChild<LSUnit>(self.Caster);
            EffectExecutor.Execute(self.TbBuffRow.EnterEffect, caster, self.Owner);
        }

        [EntitySystem]
        private static void Destroy(this Buff self)
        {
            LSUnitComponent unitComponent = self.Owner.GetParent<LSWorld>().GetComponent<LSUnitComponent>();
            LSUnit caster = unitComponent.GetChild<LSUnit>(self.Caster);
            EffectExecutor.Execute(self.TbBuffRow.FinishEffect, caster, self.Owner);
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this Buff self)
        {
            if (TimeInfo.Instance.ServerNow() > self.StartTime + self.TbBuffRow.Duration)
            {
                self.GetParent<BuffComponent>().RemoveBuff(self.BuffId, true);
                return;
            }

            if (TimeInfo.Instance.ServerNow() > self.IntervalTime)
            {
                LSUnitComponent unitComponent = self.Owner.GetParent<LSWorld>().GetComponent<LSUnitComponent>();
                LSUnit caster = unitComponent.GetChild<LSUnit>(self.Caster);
                EffectExecutor.Execute(self.TbBuffRow.IntervalEffect, caster, self.Owner);
                self.IntervalTime = TimeInfo.Instance.ServerNow() + self.TbBuffRow.Interval;
            }
        }
    }
}