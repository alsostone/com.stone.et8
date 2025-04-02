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
            self.Caster = caster.Id;
            self.Target = target.Id;
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
            LSWorld world = self.Owner.GetParent<LSWorld>();
            if (reach) {
                LSUnitComponent unitComponent = world.GetComponent<LSUnitComponent>();
                LSUnit caster = unitComponent.GetChild<LSUnit>(self.Caster);
                LSUnit target = unitComponent.GetChild<LSUnit>(self.Target);
                EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, caster, target, self.Owner);
            }
            EventSystem.Instance.Publish(world, new LSUnitRemove() { Id = self.Owner.Id });
            self.Owner.Dispose();
        }
    }
}