using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(Buff))]
    [EntitySystemOf(typeof(Buff))]
    [FriendOf(typeof(Buff))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this Buff self, int BuffId, int layerCount, LSUnit caster)
        {self.LSRoom()?.ProcessLog.LogFunction(41, self.LSParent().Id, BuffId, layerCount, caster.Id);
            self.BuffId = BuffId;
            self.LayerCount = layerCount;
            self.Caster = caster.Id;
            self.EndTime = self.TbBuffRow.Duration > 0 ? self.LSWorld().ElapsedTime + self.TbBuffRow.Duration * FP.EN3 : -1;

            LSUnit lsOwner = self.LSOwner();
            if (self.TbBuffRow.EnterEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.EnterEffect, caster, lsOwner, lsOwner, self.LayerCount);
            }
            if (self.TbBuffRow.IntervalEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.IntervalEffect, caster, lsOwner, lsOwner, self.LayerCount);
                self.IntervalTime = self.LSWorld().ElapsedTime + self.TbBuffRow.Interval * FP.EN3;
            }
        }

        [EntitySystem]
        private static void Destroy(this Buff self)
        {self.LSRoom()?.ProcessLog.LogFunction(40, self.LSParent().Id);
            LSUnit lsOwner = self.LSOwner();
            LSUnit lsCaster = self.LSUnit(self.Caster);
            if (self.TbBuffRow.EnterEffect > 0 && self.LayerCount >= 0) {
                EffectExecutor.ReverseExecute(self.TbBuffRow.EnterEffect, lsCaster, lsOwner, self.LayerCount);
            }
            if (self.TbBuffRow.FinishEffect > 0) {
                EffectExecutor.Execute(self.TbBuffRow.FinishEffect, lsCaster, lsOwner, lsOwner, self.LayerCount);
            }
        }

        [LSEntitySystem]
        private static void LSUpdate(this Buff self)
        {self.LSRoom()?.ProcessLog.LogFunction(38, self.LSParent().Id);
            // BUFF的到期由BuffComponent处理
            // BUFF DOT
            if (self.TbBuffRow.IntervalEffect > 0 && self.LSWorld().ElapsedTime >= self.IntervalTime) {
                LSUnit lsOwner = self.LSOwner();
                LSUnit lsCaster = self.LSUnit(self.Caster);
                EffectExecutor.Execute(self.TbBuffRow.IntervalEffect, lsCaster, lsOwner, lsOwner, self.LayerCount);
                self.IntervalTime += self.TbBuffRow.Interval * FP.EN3;
            }
        }
        
        public static void ResetEndFrame(this Buff self)
        {self.LSRoom()?.ProcessLog.LogFunction(39, self.LSParent().Id);
            if (self.TbBuffRow.Duration > 0)
                self.EndTime = self.LSWorld().ElapsedTime + self.TbBuffRow.Duration * FP.EN3;
        }

        public static void IncrLayerCount(this Buff self, int layerCount)
        {self.LSRoom()?.ProcessLog.LogFunction(165, self.LSParent().Id, layerCount);
            if (self.TbBuffRow.MaxLayer > 0 && self.LayerCount + layerCount > self.TbBuffRow.MaxLayer) {
                layerCount = self.TbBuffRow.MaxLayer - self.LayerCount;
            }
            self.LayerCount += layerCount;
            if (self.TbBuffRow.EnterEffect > 0) {
                LSUnit lsOwner = self.LSOwner();
                LSUnit lsCaster = self.LSUnit(self.Caster);
                EffectExecutor.Execute(self.TbBuffRow.EnterEffect, lsCaster, lsOwner, lsOwner, layerCount);
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