namespace ET
{
    [EntitySystemOf(typeof(DeathComponent))]
    [FriendOf(typeof(DeathComponent))]
    public static partial class DeathComponentSystem
    {
        [EntitySystem]
        private static void Awake(this DeathComponent self, bool isDeadRelease)
        {self.LSRoom()?.ProcessLog.LogFunction(32, self.LSParent().Id, isDeadRelease ? 1 : 0);
            self.IsDeadRelease = isDeadRelease;
        }
        
        [EntitySystem]
        private static void Destroy(this DeathComponent self)
        {
        }
        
        public static void DoDeath(this DeathComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(31, self.LSParent().Id);
            // 死亡经验分配
            //ExpGetHelper.ExpGetDead(attacker, entity);

            // entity.ComDeathDrop?.DoDrop();
            //
            // entity.ComActive?.SetActive(false);
            // entity.ComStatus?.AddAllStatus();
            //
            // entity.ComReborn?.DoReborn();
            // entity.ComState?.ChangeState(StateType.Dead);
            // entity.ComContainer?.DeathContent();
            // BattleWorld.Instance.BattleEventMgr.DispatchEvent((int)BattleEventType.EntityDeath, entity);

            if (self.IsDeadRelease)
            {
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitRemove() { Id = self.LSOwner().Id });
                self.LSOwner().Dispose();
            }
        }
    }
}