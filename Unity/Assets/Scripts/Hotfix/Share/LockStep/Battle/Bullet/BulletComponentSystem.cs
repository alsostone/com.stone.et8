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
        {self.LSRoom()?.ProcessLog.LogFunction(167, self.LSParent().Id, bulletId, caster.Id);
            self.BulletId = bulletId;
            self.EndTime = self.LSWorld().ElapsedTime + self.TbBulletRow.Life * FP.EN3;
            self.Caster = caster.Id;
            self.TowardType = ETrackTowardType.Direction;
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
            self.EndTime = self.LSWorld().ElapsedTime + self.TbBulletRow.Life * FP.EN3;
            self.Caster = caster.Id;
            self.TowardType = ETrackTowardType.Target;
            self.Target = target.Id;
        }

        [EntitySystem]
        private static void Awake(this BulletComponent self, ETrackTowardType towardType , int bulletId, LSUnit caster)
        {self.LSRoom()?.ProcessLog.LogFunction(166, self.LSParent().Id, bulletId, caster.Id);
            self.BulletId = bulletId;
            self.EndTime = self.LSWorld().ElapsedTime + self.TbBulletRow.Life * FP.EN3;
            self.Caster = caster.Id;
            self.TowardType = towardType;
        }

        [LSEntitySystem]
        private static void LSUpdate(this BulletComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(43, self.LSParent().Id);
            if (self.LSWorld().ElapsedTime >= self.EndTime)
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
            
            // 固定朝向型子弹（波浪型）需要在飞行过程中检测命中目标
            switch (self.TowardType)
            {
                case ETrackTowardType.Direction:
                    self.TryCollisionTargets(false);
                    break;
                case ETrackTowardType.Direction2:
                    self.TryCollisionTargets2(false);
                    break;
            }
        }
        
        private static void OnReachTarget(this BulletComponent self, bool reach)
        {self.LSRoom()?.ProcessLog.LogFunction(42, self.LSParent().Id, reach ? 1 : 0);
            LSUnit lsOwner = self.LSOwner();
            if (reach) {
                switch (self.TowardType)
                {
                    case ETrackTowardType.Direction:
                        self.TryCollisionTargets(true);
                        break;
                    case ETrackTowardType.Direction2:
                        self.TryCollisionTargets2(true);
                        break;
                    case ETrackTowardType.Target:
                    {
                        if (self.Target != 0) {
                            LSUnit lsCaster = self.LSUnit(self.Caster);
                            LSUnit target = self.LSUnit(self.Target);
                            EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, lsCaster, target, lsOwner);
                        }
                        break;
                    }
                    case ETrackTowardType.Position:
                    {
                        // 没有目标的子弹(固定位置型)用于范围伤害，必须接重新索敌效果，所以谁作为Target都可以，它不会被用到
                        LSUnit lsCaster = self.LSUnit(self.Caster);
                        EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, lsCaster, lsCaster, lsOwner);
                        break;
                    }
                }
            }
            LSWorld lsWorld = self.LSWorld();
            lsOwner.Dispose();
            EventSystem.Instance.Publish(lsWorld, new LSUnitRemove() { Id = lsOwner.Id });
        }

        private static void TryCollisionTargets(this BulletComponent self, bool reach)
        {self.LSRoom()?.ProcessLog.LogFunction(92, self.LSParent().Id, reach ? 1 : 0);
            if (self.SearchUnits == null || self.SearchUnits.Count == 0)
                return;
            
            LSUnit lsOwner = self.LSOwner();
            LSUnit lsCaster = self.LSUnit(self.Caster);
            if (reach)
            {
                for (int index = self.HitSearchIndex; index < self.SearchUnits.Count; index++) {
                    SearchUnitPackable searchUnitPackable = self.SearchUnits[index];
                    LSUnit target = self.LSUnit(searchUnitPackable.Target);
                    EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, lsCaster, target, lsOwner);
                }
            }
            else
            {
                TrackComponent trackComponent = lsOwner.GetComponent<TrackComponent>();
                FP sqrDistance = trackComponent.ElapsedDistance * trackComponent.ElapsedDistance;
                
                for (int index = self.HitSearchIndex; index < self.SearchUnits.Count; index++) {
                    SearchUnitPackable searchUnitPackable = self.SearchUnits[index];
                    LSUnit target = self.LSUnit(searchUnitPackable.Target);
                    if (searchUnitPackable.SqrDistance > sqrDistance)
                        break;
                    EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, lsCaster, target, lsOwner);
                    self.HitSearchIndex++;
                }
            }
        }

        private static void TryCollisionTargets2(this BulletComponent self, bool reach)
        {self.LSRoom()?.ProcessLog.LogFunction(26, self.LSParent().Id, reach ? 1 : 0);
            LSUnit lsOwner = self.LSOwner();
            List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
            CollisionComponent collisionComponent = lsOwner.GetComponent<CollisionComponent>();
            collisionComponent.GetCollisionTargets(targets, reach);
            if (targets.Count > 0)
            {
                LSUnit lsCaster = self.LSUnit(self.Caster);
                foreach (SearchUnit searchUnit in targets)
                {
                    LSUnit target = searchUnit.Target;
                    EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, lsCaster, target, lsOwner);
                }
            }
            targets.Clear();
            ObjectPool.Instance.Recycle(targets);
        }
        
    }
}