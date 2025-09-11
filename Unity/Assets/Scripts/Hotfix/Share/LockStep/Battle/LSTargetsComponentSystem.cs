using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSTargetsComponent))]
    [FriendOf(typeof(LSTargetsComponent))]
    [FriendOf(typeof(TeamComponent))]
    public static partial class LSTargetsComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSTargetsComponent self)
        {

        }

        public static void AddTarget(this LSTargetsComponent self, TeamType teamType, LSUnit lsUnit)
        {self.LSRoom()?.ProcessLog.LogFunction(3, self.LSParent().Id, lsUnit.Id);
            if (!self.TeamLSUnitsMap.TryGetValue(teamType, out List<EntityRef<LSUnit>> lsUnits))
            {
                lsUnits = new List<EntityRef<LSUnit>>();
                self.TeamLSUnitsMap[teamType] = lsUnits;
            }
            lsUnits.Add(lsUnit);
        }
        
        public static void GetAllAttackTargets(this LSTargetsComponent self, List<SearchUnit> results)
        {self.LSRoom()?.ProcessLog.LogFunction(2, self.LSParent().Id);
            for (TeamType team = TeamType.None; team < TeamType.Max; team++)
            {
                if (self.TeamLSUnitsMap.TryGetValue(team, out var targets))
                {
                    for(int i = targets.Count - 1; i >= 0; i--)
                    {
                        LSUnit target = targets[i];
                        if (target == null || target.DeadMark > 0) {
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
        {self.LSRoom()?.ProcessLog.LogFunction(1, self.LSParent().Id, range.V);
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    if (target.Active)
                    {
                        FP range2 = range + target.GetComponent<PropComponent>().Radius;
                        var dis = (target.GetComponent<TransformComponent>().Position - center).sqrMagnitude;
                        if ((range2 * range2) >= dis) {
                            results.Add(new SearchUnit() { Target = target, Distance = dis });
                        }
                    }
                }
            }
        }

        public static int GetAliveCount(this LSTargetsComponent self, TeamType teamFlag)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
                return targets.Count;
            return 0;
        }
    }
}