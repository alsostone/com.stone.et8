
using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [LSEntitySystemOf(typeof(BulletComponent))]
    [EntitySystemOf(typeof(BulletComponent))]
    [FriendOf(typeof(BulletComponent))]
    public static partial class BulletComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BulletComponent self, int bulletId, LSUnit caster, LSUnit target)
        {
            self.BulletId = bulletId;
            self.ElapseTime = TimeInfo.Instance.ServerNow() + self.TbBulletRow.Life;
            self.Caster = caster;
            self.Target = target;
        }
        
        [EntitySystem]
        private static void Destroy(this BulletComponent self)
        {
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this BulletComponent self)
        {
            if (TimeInfo.Instance.ServerNow() > self.ElapseTime)
            {
                self.OnReachTarget(false);
            }
        }
        
        private static void OnReachTarget(this BulletComponent self, bool reach)
        {
            if (reach) {
                EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, self.Caster, self.Target, self.Owner);
            }
            self.Owner.Dispose();
        }
    }
}