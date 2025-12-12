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
        
        public static void GetAttackTargetsWithShape(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, TSVector center, TSVector forward, TSVector up, FP range, List<SearchUnit> results)
        {
            switch (res.Shape)
            {
                case ESearchTargetShape.None:
                case ESearchTargetShape.Circle:
                    self.GetAttackTargetsWithCircle(teamFlag, center, range, results);
                    break;
                case ESearchTargetShape.Sector:
                    if (res.ShapeParam.Length >= 2) {
                        int angle = res.ShapeParam[1];
                        self.GetAttackTargetsWithSector(teamFlag, angle, center, forward, range, results);
                    } else {
                        self.GetAttackTargetsWithCircle(teamFlag, center, range, results);
                    }
                    break;
                case ESearchTargetShape.Rectangle:
                    if (res.ShapeParam.Length >= 2) {
                        FP halfHeight = res.ShapeParam[0] > 0 ? res.ShapeParam[0] * FP.EN4 : range;
                        FP halfWidth = res.ShapeParam[1] * FP.EN4;
                        self.GetAttackTargetsWithRectangle(teamFlag, halfWidth, halfHeight, center, results);
                    } else {
                        self.GetAttackTargetsWithRectangle(teamFlag, range, range, center, results);
                    }
                    break;
                case ESearchTargetShape.Cross:
                    if (res.ShapeParam.Length >= 2) {
                        FP halfHeight = res.ShapeParam[0] > 0 ? res.ShapeParam[0] * FP.EN4 : range;
                        FP halfWidth = res.ShapeParam[1] * FP.EN4;
                        self.GetAttackTargetsWithCross(teamFlag, halfWidth, halfHeight, center, results);
                    } else {
                        self.GetAttackTargetsWithCross(teamFlag, FP.Half, range, center, results);
                    }
                    break;
                case ESearchTargetShape.Line:
                    if (res.ShapeParam.Length >= 2) {
                        FP halfHeight = res.ShapeParam[0] > 0 ? res.ShapeParam[0] * FP.EN4 : range;
                        FP halfWidth = res.ShapeParam[1] * FP.EN4;
                        self.GetAttackTargetsWithLine(teamFlag, halfWidth, halfHeight, center, forward, up, results);
                    } else {
                        self.GetAttackTargetsWithLine(teamFlag, FP.Half, range, center, forward, up, results);
                    }
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
                    TSVector dir = (target.GetComponent<TransformComponent>().Position - center).IgnoreY();
                    FP distance = dir.sqrMagnitude;
                    if (range2 * range2 >= distance) {
                        results.Add(new SearchUnit() { Target = target, SqrDistance = distance });
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithSector(this LSTargetsComponent self, TeamType teamFlag, int angle, TSVector center, TSVector forward, FP range, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                int halfAngle = angle / 2;
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    FP range2 = range + target.GetComponent<PropComponent>().Radius;
                    TSVector dir = (target.GetComponent<TransformComponent>().Position - center).IgnoreY();
                    FP distance = dir.sqrMagnitude;
                    if (range2 * range2 >= distance)
                    {
                        FP angle2 = TSVector.Angle(forward, dir.normalized);
                        if (angle2 <= halfAngle) {
                            results.Add(new SearchUnit() { Target = target, SqrDistance = distance });
                        }
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithRectangle(this LSTargetsComponent self, TeamType teamFlag, FP halfWidth, FP halfHeight, TSVector center, List<SearchUnit> results)
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
                    TSVector dir = (target.GetComponent<TransformComponent>().Position - center).IgnoreY();
                    if (FP.Abs(dir.x) <= halfWidth && FP.Abs(dir.z) <= halfHeight) {
                        results.Add(new SearchUnit() { Target = target, SqrDistance = dir.sqrMagnitude});
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithCross(this LSTargetsComponent self, TeamType teamFlag, FP halfWidth, FP halfHeight, TSVector center, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                FP halfWidth2 = halfHeight;
                FP halfHeight2 = halfWidth;
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    TSVector dir = (target.GetComponent<TransformComponent>().Position - center).IgnoreY();
                    TSVector absDir = TSVector.Abs(dir);
                    if ((absDir.x <= halfWidth && absDir.z <= halfHeight) ||
                        (absDir.x <= halfWidth2 && absDir.z <= halfHeight2)) {
                        results.Add(new SearchUnit() { Target = target, SqrDistance = dir.sqrMagnitude });
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithLine(this LSTargetsComponent self, TeamType teamFlag, FP halfWidth, FP height, TSVector center, TSVector forward, TSVector up, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                TSVector right = TSVector.Cross(up, forward).normalized;
                for(int i = targets.Count - 1; i >= 0; i--)
                {
                    LSUnit target = targets[i];
                    if (target == null || target.DeadMark > 0) {
                        targets.RemoveAt(i);
                        continue;
                    }
                    TSVector dir = (target.GetComponent<TransformComponent>().Position - center).IgnoreY();
                    FP forwardDist = TSVector.Dot(forward, dir);
                    FP rightDist = TSVector.Dot(right, dir);
                    if (FP.Abs(rightDist) <= halfWidth && forwardDist >= 0 && forwardDist <= height) {
                        results.Add(new SearchUnit() { Target = target, SqrDistance = dir.sqrMagnitude });
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