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
        private static void Awake(this BulletComponent self, int bulletId, LSUnit caster, LSUnit target)
        {
            self.BulletId = bulletId;
            self.ElapseFrame = self.LSWorld().Frame + self.TbBulletRow.Life.Convert2Frame();
            self.Caster = caster.Id;
            self.Target = target.Id;
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this BulletComponent self)
        {
            if (self.LSWorld().Frame > self.ElapseFrame)
            {
                self.OnReachTarget(false);
                return;
            }

            // 子弹的轨迹执行完毕后判定为已命中
            TrackComponent trackComponent = self.LSOwner().GetComponent<TrackComponent>();
            if (trackComponent.Duration > 0 && trackComponent.EclipseTime >= trackComponent.Duration)
            {
                self.OnReachTarget(true);
                return;
            }

            // 子弹足够靠近目标时判定为已命中
            FP speedPerFrame = trackComponent.HorSpeed * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            TransformComponent transformTarget = self.LSUnit(self.Target).GetComponent<TransformComponent>();
            TransformComponent transformBullet = self.LSOwner().GetComponent<TransformComponent>();
            if (speedPerFrame * speedPerFrame > (transformTarget.Position - transformBullet.Position).sqrMagnitude)
            {
                self.OnReachTarget(true);
                return;
            }
        }
        
        private static void OnReachTarget(this BulletComponent self, bool reach)
        {
            if (reach) {
                LSUnit caster = self.LSUnit(self.Caster);
                LSUnit target = self.LSUnit(self.Target);
                EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, caster, target, self.LSOwner());
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSUnitRemove() { Id = self.LSOwner().Id });
            self.LSOwner().Dispose();
        }
    }
}