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
            self.ElapseFrame = self.LSWorld().Frame + self.TbBulletRow.Life.Convert2Frame();
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
            if (self.LSWorld().Frame > self.ElapseFrame)
            {
                self.OnReachTarget(false);
            }
        }
        
        private static void OnReachTarget(this BulletComponent self, bool reach)
        {
            if (reach) {
                LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
                LSUnit caster = unitComponent.GetChild<LSUnit>(self.Caster);
                LSUnit target = unitComponent.GetChild<LSUnit>(self.Target);
                EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, caster, target, self.LSOwner());
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSUnitRemove() { Id = self.LSOwner().Id });
            self.LSOwner().Dispose();
        }
    }
}