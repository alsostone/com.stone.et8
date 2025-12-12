namespace ET
{
    [EntitySystemOf(typeof(DeathComponent))]
    [LSEntitySystemOf(typeof(DeathComponent))]
    [FriendOf(typeof(DeathComponent))]
    public static partial class DeathComponentSystem
    {
        [EntitySystem]
        private static void Awake(this DeathComponent self, bool isDeadRelease)
        {self.LSRoom()?.ProcessLog.LogFunction(62, self.LSParent().Id, isDeadRelease ? 1 : 0);
            self.IsDeadRelease = isDeadRelease;
        }
        
        [EntitySystem]
        private static void Destroy(this DeathComponent self)
        {
        }

        [LSEntitySystem]
        private static void LSUpdate(this DeathComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(61, self.LSParent().Id);
            LSUnit lsUnit = self.LSOwner();
            if (lsUnit.DeadMark == 1)
            {
                // 血量归零触发死亡技能
                SkillComponent skillComponent = self.LSOwner().GetComponent<SkillComponent>();
                skillComponent.ForceAllDone();
                skillComponent.TryCastSkill(ESkillType.Dead);
                lsUnit.DeadMark = 2;
            }

            if (lsUnit.DeadMark == 2)
            {
                SkillComponent skillComponent = self.LSOwner().GetComponent<SkillComponent>();
                if (skillComponent.HasRunningSkill())
                    return;
                self.DoDeathReal();
                lsUnit.DeadMark = 3;
            }
        }

        private static void DoDeathReal(this DeathComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(60, self.LSParent().Id);
            LSUnit lsOwner = self.LSOwner();
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
                LSWorld lsWorld = self.LSWorld();
                lsOwner.Dispose();
                EventSystem.Instance.Publish(lsWorld, new LSUnitRemove() { Id = lsOwner.Id });
            }
        }
    }
}