using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSTargetsComponent))]
    [FriendOf(typeof(LSTargetsComponent))]
    public static partial class LSTargetsComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSTargetsComponent self)
        {

        }

        [EntitySystem]
        private static void Deserialize(this LSTargetsComponent self)
        {
            foreach (var pair in self.TeamLSUnitsMap)
            {
                pair.Value.Clear();
            }

            LSUnitComponent component = self.LSWorld().GetComponent<LSUnitComponent>();
            foreach (var pair in component.Children)
            {
                if (pair.Value is LSUnit lsUnit)
                {
                    // 只有拥有 BeHitComponent 的单位才会被添加到目标列表 和BeHitComponent的Awake中添加到目标列表一致
                    if (lsUnit.GetComponent<BeHitComponent>() == null)
                        continue;
                    TeamType type = lsUnit.GetComponent<TeamComponent>().GetTeamType();
                    self.AddTarget(type, lsUnit);
                }
            }
        }
        
        public static void AddTarget(this LSTargetsComponent self, TeamType teamType, LSUnit lsUnit)
        {
            if (!self.TeamLSUnitsMap.TryGetValue(teamType, out List<EntityRef<LSUnit>> lsUnits))
            {
                lsUnits = new List<EntityRef<LSUnit>>();
                self.TeamLSUnitsMap[teamType] = lsUnits;
            }
            lsUnits.Add(lsUnit);
        }
        
        public static void GetAllAttackTargets(this LSTargetsComponent self, List<SearchUnit> results)
        {
            for (TeamType team = TeamType.None; team < TeamType.Max; team++)
            {
                if (self.TeamLSUnitsMap.TryGetValue(team, out var targets))
                {
                    for(int i = targets.Count - 1; i >= 0; i--)
                    {
                        LSUnit target = targets[i];
                        if (target == null || target.DeadMark) {
                            targets.RemoveAt(i);
                            continue;
                        }
                        if (target.Active) {
                            results.Add(new SearchUnit() { Target = target });
                        }
                    }
                }
            }
        }
        
        public static void GetAttackTargets(this LSTargetsComponent self, TeamType teamFlag, TSVector center, FP range, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    if (target.Active)
                    {
                        var dis = (target.GetComponent<TransformComponent>().Position - center).sqrMagnitude;
                        var sqrRange = target.GetAttackSqrRange(range);
                        if (sqrRange >= dis) {
                            results.Add(new SearchUnit() { Target = target, Distance = dis });
                        }
                    }
                }
            }
        }
    }
}