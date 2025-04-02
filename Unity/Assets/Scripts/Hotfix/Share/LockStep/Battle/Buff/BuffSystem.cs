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
            self.StartFrame = self.LSWorld().Frame;
            self.IntervalFrame = self.StartFrame + self.TbBuffRow.Interval.Convert2Frame();
            self.EndFrame = self.StartFrame + self.TbBuffRow.Duration.Convert2Frame();
            self.LayerCount = 1;
            
            LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            LSUnit caster = unitComponent.GetChild<LSUnit>(self.Caster);
            EffectExecutor.Execute(self.TbBuffRow.EnterEffect, caster, self.LSOwner());
        }

        [EntitySystem]
        private static void Destroy(this Buff self)
        {
            LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            LSUnit caster = unitComponent.GetChild<LSUnit>(self.Caster);
            EffectExecutor.Execute(self.TbBuffRow.FinishEffect, caster, self.LSOwner());
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this Buff self)
        {
            if (self.LSWorld().Frame > self.EndFrame)
            {
                self.GetParent<BuffComponent>().RemoveBuff(self.BuffId, true);
                return;
            }

            if (self.LSWorld().Frame > self.IntervalFrame)
            {
                LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
                LSUnit caster = unitComponent.GetChild<LSUnit>(self.Caster);
                EffectExecutor.Execute(self.TbBuffRow.IntervalEffect, caster, self.LSOwner());
                self.IntervalFrame = self.LSWorld().Frame + self.TbBuffRow.Interval.Convert2Frame();
            }
        }

        public static void ResetEndFrame(this Buff self)
        {
            self.EndFrame = self.LSWorld().Frame + self.TbBuffRow.Duration.Convert2Frame();
        }
    }
}