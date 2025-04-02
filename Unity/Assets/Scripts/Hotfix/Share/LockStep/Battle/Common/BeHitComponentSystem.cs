namespace ET
{
    [EntitySystemOf(typeof(BeHitComponent))]
    [FriendOf(typeof(BeHitComponent))]
    public static partial class BeHitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BeHitComponent self)
        {
            
        }
        
        [EntitySystem]
        private static void Destroy(this BeHitComponent self)
        {
            self.Attackers.Clear();
        }
        
        public static void BeDamage(this BeHitComponent self, LSUnit attacker, long damage)
        {
            // 处理是否免疫、格挡、闪避等
            // ...
            
            // 记录进攻者 & 掉血
            self.AddAttacker(attacker.Id);
            self.DeductHp(attacker, damage);
        }

        private static void DeductHp(this BeHitComponent self, LSUnit attacker, long value)
        {
            var component = self.LSOwner().GetComponent<PropComponent>();
            var hp = component.GetByKey(NumericType.Hp);
            if (hp <= 0) { return; }

            var current = hp - value;
            component.Set(NumericType.Hp, current);
            if (current <= 0) {
                // 死亡经验分配
                //ExpGetHelper.ExpGetDead(attacker, entity);
                
                // 血量值空触发技能
                var comSkill = self.LSOwner().GetComponent<SkillComponent>();
                if (comSkill != null)
                {
                    comSkill.ForceAllDone();
                    comSkill.TryCastSkill(ESkillType.Dead);
                }
            
                // 血量值空则死亡
                var comDeath = self.LSOwner().GetComponent<DeathComponent>();
                if (comDeath != null) {
                    comDeath.DoDeath();
                } else {
                    EventSystem.Instance.Publish(self.LSWorld(), new LSUnitRemove() { Id = self.LSOwner().Id });
                    self.LSOwner().Dispose();
                }
            }
        }
        
        public static void AddAttacker(this BeHitComponent self, long attacker)
        {
            if (!self.Attackers.Contains(attacker)) {
                self.Attackers.Add(attacker);
            }
        }

    }
}