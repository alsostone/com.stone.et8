namespace ET
{
    [EntitySystemOf(typeof(DeathComponent))]
    [FriendOf(typeof(DeathComponent))]
    public static partial class DeathComponentSystem
    {
        [EntitySystem]
        private static void Awake(this DeathComponent self, bool isDeadRelease)
        {
            self.IsDeadRelease = isDeadRelease;
        }
        
        [EntitySystem]
        private static void Destroy(this DeathComponent self)
        {
        }
        
        public static void DoDeath(this DeathComponent self)
        {
            if (self.Dead) { return; }
            self.Dead = true;
            
            // entity.ComDeathDrop?.DoDrop();
            //
            // entity.ComActive?.SetActive(false);
            // entity.ComStatus?.AddAllStatus();
            //
            // entity.ComReborn?.DoReborn();
            // entity.ComState?.ChangeState(StateType.Dead);
            // entity.ComContainer?.DeathContent();
            // BattleWorld.Instance.BattleEventMgr.DispatchEvent((int)BattleEventType.EntityDeath, entity);

            if (self.IsDeadRelease) {
                EventSystem.Instance.Publish(self.Owner.GetParent<LSWorld>(), new LSUnitRemove() { Id = self.Owner.Id });
                self.Owner.Dispose();
            }
        }
        
    }
}