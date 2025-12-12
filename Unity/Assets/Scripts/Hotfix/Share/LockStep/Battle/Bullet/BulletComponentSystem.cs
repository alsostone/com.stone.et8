using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(BulletComponent))]
    [EntitySystemOf(typeof(BulletComponent))]
    [FriendOf(typeof(BulletComponent))]
    [FriendOf(typeof(TrackComponent))]
    public static partial class BulletComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BulletComponent self, int bulletId, LSUnit caster, List<SearchUnit> targets)
        {
            self.BulletId = bulletId;
            self.OverFrame = self.LSWorld().Frame + self.TbBulletRow.Life.Convert2Frame();
            self.Caster = caster.Id;
            self.SearchUnits = new List<SearchUnitPackable>();
            foreach (SearchUnit searchUnit in targets)
            {
                self.SearchUnits.Add(new SearchUnitPackable()
                {
                    Target = searchUnit.Target.Id,
                    SqrDistance = searchUnit.SqrDistance
                });
            }
        }
        
        [EntitySystem]
        private static void Awake(this BulletComponent self, int bulletId, LSUnit caster, LSUnit target)
        {self.LSRoom()?.ProcessLog.LogFunction(44, self.LSParent().Id, bulletId, caster.Id, target.Id);
            self.BulletId = bulletId;
            self.OverFrame = self.LSWorld().Frame + self.TbBulletRow.Life.Convert2Frame();
            self.Caster = caster.Id;
            self.Target = target.Id;
        }

        [EntitySystem]
        private static void Awake(this BulletComponent self, int bulletId, LSUnit caster)
        {
            self.BulletId = bulletId;
            self.OverFrame = self.LSWorld().Frame + self.TbBulletRow.Life.Convert2Frame();
            self.Caster = caster.Id;
        }

        [LSEntitySystem]
        private static void LSUpdate(this BulletComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(43, self.LSParent().Id);
            if (self.LSWorld().Frame > self.OverFrame)
            {
                self.OnReachTarget(false);
                return;
            }

            // 子弹的轨迹执行完毕后判定为已命中
            TrackComponent trackComponent = self.LSOwner().GetComponent<TrackComponent>();
            if (trackComponent.IsReached)
            {
                self.OnReachTarget(true);
                return;
            }
            
            // 若指定的是目标组，说明是固定朝向型子弹（波浪型）
            if (self.SearchUnits != null && self.SearchUnits.Count > 0)
            {
                FP sqrDistance = trackComponent.ElapsedDistance * trackComponent.ElapsedDistance;
                LSUnit caster = self.LSUnit(self.Caster);
                
                for (int index = self.HitSearchIndex; index < self.SearchUnits.Count; index++) {
                    SearchUnitPackable searchUnitPackable = self.SearchUnits[index];
                    LSUnit target = self.LSUnit(searchUnitPackable.Target);
                    if (searchUnitPackable.SqrDistance > sqrDistance)
                        break;
                    EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, caster, target, self.LSOwner());
                    self.HitSearchIndex++;
                }
            }
        }
        
        [EntitySystem]
        private static void Destroy(this BulletComponent self)
        {
        }
        
        private static void OnReachTarget(this BulletComponent self, bool reach)
        {self.LSRoom()?.ProcessLog.LogFunction(42, self.LSParent().Id, reach ? 1 : 0);
            LSUnit lsOwner = self.LSOwner();
            if (reach) {
                LSUnit caster = self.LSUnit(self.Caster);
                
                if (self.SearchUnits != null && self.SearchUnits.Count > 0)
                {
                    for (int index = self.HitSearchIndex; index < self.SearchUnits.Count; index++)
                    {
                        SearchUnitPackable searchUnitPackable = self.SearchUnits[index];
                        LSUnit target = self.LSUnit(searchUnitPackable.Target);
                        EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, caster, target, lsOwner);
                    }
                }
                else if (self.Target != 0)
                {
                    LSUnit target = self.LSUnit(self.Target);
                    EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, caster, target, lsOwner);
                }
                else
                {
                    // 没有目标的子弹(固定位置型)用于范围伤害，必须接重新索敌效果，所以谁作为Target都可以，它不会被用到
                    EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, caster, caster, lsOwner);
                }
            }
            LSWorld lsWorld = self.LSWorld();
            lsOwner.Dispose();
            EventSystem.Instance.Publish(lsWorld, new LSUnitRemove() { Id = lsOwner.Id });
        }
    }
}