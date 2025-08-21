using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(BeHitComponent))]
    [FriendOf(typeof(BeHitComponent))]
    [FriendOf(typeof(TeamComponent))]
    public static partial class BeHitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BeHitComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(30, self.LSParent().Id);
            LSTargetsComponent component = self.LSWorld().GetComponent<LSTargetsComponent>();
            TeamType teamType = self.LSOwner().GetComponent<TeamComponent>().Type;
            component.AddTarget(teamType, self.LSOwner());
        }
        
        [EntitySystem]
        private static void Destroy(this BeHitComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(29, self.LSParent().Id);
            self.Attackers.Clear();
        }
        
        [EntitySystem]
        private static void Deserialize(this BeHitComponent self)
        {
            LSTargetsComponent component = self.LSWorld().GetComponent<LSTargetsComponent>();
            TeamType teamType = self.LSOwner().GetComponent<TeamComponent>().Type;
            component.AddTarget(teamType, self.LSOwner());
        }
        
        public static void BeHealing(this BeHitComponent self, LSUnit attacker, FP value)
        {self.LSRoom()?.ProcessLog.LogFunction(27, self.LSParent().Id, attacker.Id, value.V);
            if (self.LSOwner().DeadMark > 0) { return; }
            
            PropComponent component = self.LSOwner().GetComponent<PropComponent>();
            component.AddRealProp(NumericType.Hp, value);
            EventSystem.Instance.Publish(self.LSWorld(), new LSUnitFloating() { Id = self.LSOwner().Id, Value = value, Type = FloatingType.Heal});
        }

        public static void BeDamage(this BeHitComponent self, LSUnit attacker, FP damage)
        {self.LSRoom()?.ProcessLog.LogFunction(26, self.LSParent().Id, attacker.Id, damage.V);
            if (self.LSOwner().DeadMark > 0) { return; }
            
            // 1. 优先处理闪避
            PropComponent component = self.LSOwner().GetComponent<PropComponent>();
            FP evasion = component.Get(NumericType.EvasionRate);
            if (evasion > 0)
            {
                FP random = (FP)self.GetRandom().Next(0, LSConstValue.Probability) / LSConstValue.Probability;
                if (random < evasion)
                {
                    EventSystem.Instance.Publish(self.LSWorld(), new LSUnitFloating() { Id = self.LSOwner().Id, Value = damage, Type = FloatingType.Miss});
                    return;
                }
            }
            
            // 2. 计算暴击伤害
            bool isCritical = false;
            FP critRate = component.Get(NumericType.CriticalRate);
            if (critRate > 0)
            {
                FP random = (FP)self.GetRandom().Next(0, LSConstValue.Probability) / LSConstValue.Probability;
                if (random < critRate)
                {
                    isCritical = true;
                    damage *= component.Get(NumericType.CriticalDamagePercent);
                }
            }
            
            // 3. 攻防计算 多人合作时伤害使用攻-防，以使得攻防属性变化敏感度最高。 最低1点
            FP defense = component.Get(NumericType.Def);
            damage = TSMath.Max(damage - defense, FP.One);
            
            // 4. 计算格挡减伤
            bool isBlock = false;
            FP blockRate = component.Get(NumericType.BlockRate);
            if (blockRate > 0)
            {
                FP random = (FP)self.GetRandom().Next(0, LSConstValue.Probability) / LSConstValue.Probability;
                if (random < blockRate)
                {
                    isBlock = true;
                    damage *= component.Get(NumericType.BlockDamagePercent);
                }
            }
            
            if (isCritical) {
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitFloating() { Id = self.LSOwner().Id, Value = damage, Type = FloatingType.Damage });
            } else if (isBlock) {
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitFloating() { Id = self.LSOwner().Id, Value = damage, Type = FloatingType.Block });
            } else {
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitFloating() { Id = self.LSOwner().Id, Value = damage, Type = FloatingType.Damage });
            }

            // 记录进攻者 & 掉血
            self.AddAttacker(attacker.Id);
            self.DeductHp(attacker, damage);
        }

        private static void DeductHp(this BeHitComponent self, LSUnit attacker, FP value)
        {self.LSRoom()?.ProcessLog.LogFunction(25, self.LSParent().Id, attacker.Id, value.V);
            PropComponent component = self.LSOwner().GetComponent<PropComponent>();
            FP hp = component.Get(NumericType.Hp);
            if (hp <= FP.EN1) { return; }

            FP current = hp - value;
            component.Set(NumericType.Hp, current);
            
            // 血量值小于等于0时，没有死亡组件直接移除 有则只标记死亡即可
            if (current <= FP.EN1)
            {
                self.LSOwner().DeadMark = 1;
                if (self.LSOwner().GetComponent<DeathComponent>() != null)
                {
                    EventSystem.Instance.Publish(self.LSWorld(), new LSUnitRemove() { Id = self.LSOwner().Id });
                    self.LSOwner().Dispose();
                }
            }
        }
        
        private static void AddAttacker(this BeHitComponent self, long attacker)
        {self.LSRoom()?.ProcessLog.LogFunction(24, self.LSParent().Id);
            if (!self.Attackers.Contains(attacker)) {
                self.Attackers.Add(attacker);
            }
        }

        internal static void GetCounterAttack(this BeHitComponent self, TSVector center, FP range, List<SearchUnit> results)
        {self.LSRoom()?.ProcessLog.LogFunction(94, self.LSParent().Id, range.V);
            for (var i = self.Attackers.Count - 1; i >= 0; i--)
            {
                var target = self.LSUnit(self.Attackers[i]);
                if (target == null) {
                    self.Attackers.RemoveAt(i);
                }
                else if (target.Active) {
                    FP range2 = range + target.GetComponent<PropComponent>().Radius;
                    var dis = (target.GetComponent<TransformComponent>().Position - center).sqrMagnitude;
                    if ((range2 * range2) >= dis) {
                        results.Add(new SearchUnit() { Target = target, Distance = dis });
                    }
                }
            }
        }
    }
}