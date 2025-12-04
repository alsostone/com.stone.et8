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
        {self.LSRoom()?.ProcessLog.LogFunction(26, self.LSParent().Id, lsUnit.Id);
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
                        if (target == null || target.DeadMark > 0) {
                            targets.RemoveAt(i);
                            continue;
                        }
                        results.Add(new SearchUnit() { Target = target });
                    }
                }
            }
        }
        
        public static void GetAttackTargetsWithShape(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, TSVector center, TSVector forward, FP range, List<SearchUnit> results)
        {
            switch (res.Shape)
            {
                case ESearchTargetShape.None:
                case ESearchTargetShape.Circle:
                    self.GetAttackTargetsWithCircle(teamFlag, center, range, results);
                    break;
                case ESearchTargetShape.Sector:
                    self.GetAttackTargetsWithSector(teamFlag, res, center, forward, range, results);
                    break;
                case ESearchTargetShape.Rectangle:
                    self.GetAttackTargetsWithRectangle(teamFlag, res, center, results);
                    break;
                case ESearchTargetShape.Cross:
                    self.GetAttackTargetsWithCross(teamFlag, res, center, results);
                    break;
            }
        }

        private static void GetAttackTargetsWithCircle(this LSTargetsComponent self, TeamType teamFlag, TSVector center, FP range, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    FP range2 = range + target.GetComponent<PropComponent>().Radius;
                    var distance = (target.GetComponent<TransformComponent>().Position - center).sqrMagnitude;
                    if (range2 * range2 >= distance) {
                        results.Add(new SearchUnit() { Target = target, Distance = distance });
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithSector(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, TSVector center, TSVector forward, FP range, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                FP halfAngle = (res.ShapeParam.Length >= 2 ? res.ShapeParam[1] * FP.EN4 : 360) * FP.Half;
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    TSVector dir = target.GetComponent<TransformComponent>().Position - center;
                    FP distance = dir.sqrMagnitude;
                    FP range2 = range + target.GetComponent<PropComponent>().Radius;
                    if (range2 * range2 >= distance)
                    {
                        FP angle = TSVector.Angle(forward, dir.normalized);
                        if (angle <= halfAngle) {
                            results.Add(new SearchUnit() { Target = target, Distance = distance });
                        }
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithRectangle(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, TSVector center, List<SearchUnit> results)
        {
            if (res.ShapeParam.Length < 2) return;
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                FP halfWidth = res.ShapeParam[0] * FP.EN4 * FP.Half;
                FP halfHeight = res.ShapeParam[1] * FP.EN4 * FP.Half;
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    TSVector pos = target.GetComponent<TransformComponent>().Position;
                    TSVector dir = pos - center;
                    if (FP.Abs(dir.x) <= halfWidth && FP.Abs(dir.z) <= halfHeight) {
                        results.Add(new SearchUnit() { Target = target });
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithCross(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, TSVector center, List<SearchUnit> results)
        {
            if (res.ShapeParam.Length < 2) return;
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                FP halfWidth1 = res.ShapeParam[0] * FP.EN4 * FP.Half;
                FP halfHeight1 = res.ShapeParam[1] * FP.EN4 * FP.Half;
                FP halfWidth2 = halfHeight1;
                FP halfHeight2 = halfWidth1;
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    TSVector pos = target.GetComponent<TransformComponent>().Position;
                    TSVector absDir = TSVector.Abs(pos - center);
                    if ((absDir.x <= halfWidth1 && absDir.z <= halfHeight1) ||
                        (absDir.x <= halfWidth2 && absDir.z <= halfHeight2)) {
                        results.Add(new SearchUnit() { Target = target });
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

        public static void GetAttackTargets(this LSTargetsComponent self, TeamType teamFlag, EUnitType type, TSBounds bounds, List<long> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    if ((target.GetComponent<TypeComponent>().Type & type) != 0)
                    {
                        TSVector point = target.GetComponent<TransformComponent>().Position;
                        TSVector min = bounds.min;
                        TSVector max = bounds.max;
                        if (min.x <= point.x && max.x >= point.x && min.z <= point.z && max.z >= point.z)
                        {
                            results.Add(target.Id);
                        }
                    }
                }
            }
        }
        
    }
}