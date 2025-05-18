using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(BeHitComponent))]
    [LSEntitySystemOf(typeof(BeHitComponent))]
    [FriendOf(typeof(BeHitComponent))]
    public static partial class BeHitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BeHitComponent self)
        {
            LSTargetsComponent component = self.LSWorld().GetComponent<LSTargetsComponent>();
            TeamType teamType = self.LSOwner().GetComponent<TeamComponent>().GetTeamType();
            component.AddTarget(teamType, self.LSOwner());
        }
        
        [EntitySystem]
        private static void Destroy(this BeHitComponent self)
        {
            self.Attackers.Clear();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this BeHitComponent self)
        {
            LSUnit lsUnit = self.LSOwner();
            if (lsUnit.DeadMark && !lsUnit.Dead)
            {
                // 释放死亡技能时还没有真正死亡
                SkillComponent skillComponent = self.LSOwner().GetComponent<SkillComponent>();
                if (skillComponent != null && skillComponent.HasRunningSkill())
                    return;

                self.LSOwner().Dead = true;
                DeathComponent deathComponent = self.LSOwner().GetComponent<DeathComponent>();
                if (deathComponent != null)
                    deathComponent.DoDeath();
                else
                {
                    EventSystem.Instance.Publish(self.LSWorld(), new LSUnitRemove() { Id = self.LSOwner().Id });
                    self.LSOwner().Dispose();
                }
            }
        }
        
        public static void BeDamage(this BeHitComponent self, LSUnit attacker, FP damage)
        {
            if (self.LSOwner().DeadMark) { return; }
            // 处理是否免疫、格挡、闪避等
            // ...
            
            // 记录进攻者 & 掉血
            self.AddAttacker(attacker.Id);
            self.DeductHp(attacker, damage);
        }

        private static void DeductHp(this BeHitComponent self, LSUnit attacker, FP value)
        {
            PropComponent component = self.LSOwner().GetComponent<PropComponent>();
            FP hp = component.Get(NumericType.Hp);
            if (hp <= 0) { return; }

            FP current = hp - value;
            component.Set(NumericType.Hp, current);
            EventSystem.Instance.Publish(self.LSWorld(), new LSUnitFloating() { Id = self.LSOwner().Id, Value = value, Type = FloatingType.Damage});
            if (current <= 0)
            {
                // 血量值空触发死亡技能
                SkillComponent comSkill = self.LSOwner().GetComponent<SkillComponent>();
                if (comSkill != null)
                {
                    comSkill.ForceAllDone();
                    comSkill.TryCastSkill(ESkillType.Dead);
                }
                self.LSOwner().DeadMark = true;
            }
        }
        
        private static void AddAttacker(this BeHitComponent self, long attacker)
        {
            if (!self.Attackers.Contains(attacker)) {
                self.Attackers.Add(attacker);
            }
        }

        internal static void GetCounterAttack(this BeHitComponent self, TSVector center, FP sqrRange, List<SearchUnit> results)
        {
            for (var i = self.Attackers.Count - 1; i >= 0; i--)
            {
                var target = self.LSUnit(self.Attackers[i]);
                if (target == null) {
                    self.Attackers.RemoveAt(i);
                }
                else if (target.Active) {
                    var dis = (target.Position - center).sqrMagnitude;
                    if (sqrRange >= dis) {
                        results.Add(new SearchUnit() { Target = target, Distance = dis });
                    }
                }
            }
        }
    }
}