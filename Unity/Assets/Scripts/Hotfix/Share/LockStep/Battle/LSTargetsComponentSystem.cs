using System.Collections.Generic;
using ST.Mono;
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

        public static DynamicTree<long> GetTargetsTree(this LSTargetsComponent self, TeamType teamType)
        {self.LSRoom()?.ProcessLog.LogFunction(26, self.LSParent().Id);
            if (!self.TeamLSUnitsMap.TryGetValue(teamType, out var targets))
            {
                targets = new DynamicTree<long>(32);
                self.TeamLSUnitsMap[teamType] = targets;
            }
            return targets;
        }
        
        public static void GetAttackTargetsWithShape(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, TSVector center, TSVector forward, TSVector up, FP range, List<SearchUnit> results)
        {
            switch (res.Shape)
            {
                case ESearchTargetShape.None:
                case ESearchTargetShape.Circle:
                    self.GetAttackTargetsWithCircle(teamFlag, res, center, range, results);
                    break;
                case ESearchTargetShape.Sector:
                    if (res.ShapeParam.Length >= 2) {
                        int halfAngle = res.ShapeParam[1] / 2;
                        self.GetAttackTargetsWithSector(teamFlag, res, halfAngle, center, forward, range, results);
                    } else {
                        self.GetAttackTargetsWithCircle(teamFlag, res, center, range, results);
                    }
                    break;
                case ESearchTargetShape.Rectangle:
                    if (res.ShapeParam.Length >= 2) {
                        FP halfHeight = res.ShapeParam[0] > 0 ? res.ShapeParam[0] * FP.EN4 : range;
                        FP halfWidth = res.ShapeParam[1] * FP.EN4;
                        self.GetAttackTargetsWithRectangle(teamFlag, res, halfWidth, halfHeight, center, results);
                    } else {
                        self.GetAttackTargetsWithRectangle(teamFlag, res, range, range, center, results);
                    }
                    break;
                case ESearchTargetShape.Cross:
                    if (res.ShapeParam.Length >= 2) {
                        FP halfHeight = res.ShapeParam[0] > 0 ? res.ShapeParam[0] * FP.EN4 : range;
                        FP halfWidth = res.ShapeParam[1] * FP.EN4;
                        self.GetAttackTargetsWithCross(teamFlag, res, halfWidth, halfHeight, center, results);
                    } else {
                        self.GetAttackTargetsWithCross(teamFlag, res, FP.Half, range, center, results);
                    }
                    break;
                case ESearchTargetShape.Line:
                    if (res.ShapeParam.Length >= 2) {
                        FP halfHeight = res.ShapeParam[0] > 0 ? res.ShapeParam[0] * FP.EN4 : range;
                        FP halfWidth = res.ShapeParam[1] * FP.EN4;
                        self.GetAttackTargetsWithLine(teamFlag, res, halfWidth, halfHeight, center, forward, up, results);
                    } else {
                        self.GetAttackTargetsWithLine(teamFlag, res, FP.Half, range, center, forward, up, results);
                    }
                    break;
            }
        }

        private static void GetAttackTargetsWithCircle(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, TSVector center, FP range, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                TSVector extents = new TSVector(range, range, range);
                AABB searchAABB = new AABB(center - extents, center + extents);
                foreach(var node in targets.Query(searchAABB))
                {
                    LSUnit target = self.LSUnit(node.UserData);
                    if (target == null || target.DeadMark > 0) {
                        continue;
                    }
                    if (!target.GetComponent<TypeComponent>().IsType(res.Type)) {
                        continue;
                    }
                    FP range2 = range + target.GetComponent<PropComponent>().Radius;
                    TSVector dir = target.GetComponent<TransformComponent>().Position - center;
                    FP distance = dir.sqrMagnitude;
                    if (range2 * range2 >= distance) {
                        results.Add(new SearchUnit() { Target = target, SqrDistance = distance });
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithSector(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, int halfAngle, TSVector center, TSVector forward, FP range, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                TSVector extents = new TSVector(range, range, range);
                AABB searchAABB = new AABB(center - extents, center + extents);
                foreach(var node in targets.Query(searchAABB))
                {
                    LSUnit target = self.LSUnit(node.UserData);
                    if (target == null || target.DeadMark > 0) {
                        continue;
                    }
                    if (!target.GetComponent<TypeComponent>().IsType(res.Type)) {
                        continue;
                    }
                    FP range2 = range + target.GetComponent<PropComponent>().Radius;
                    TSVector dir = target.GetComponent<TransformComponent>().Position - center;
                    FP distance = dir.sqrMagnitude;
                    if (range2 * range2 >= distance)
                    {
                        FP angle2 = TSVector.Angle(forward, dir);
                        if (angle2 <= halfAngle) {
                            results.Add(new SearchUnit() { Target = target, SqrDistance = distance });
                        }
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithRectangle(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, FP halfWidth, FP halfHeight, TSVector center, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                TSVector extents = new TSVector(halfWidth, FP.Half, halfHeight);
                AABB searchAABB = new AABB(center - extents, center + extents);
                foreach(var node in targets.Query(searchAABB))
                {
                    LSUnit target = self.LSUnit(node.UserData);
                    if (target == null || target.DeadMark > 0) {
                        continue;
                    }
                    if (!target.GetComponent<TypeComponent>().IsType(res.Type)) {
                        continue;
                    }
                    TSVector dir = target.GetComponent<TransformComponent>().Position - center;
                    if (FP.Abs(dir.x) <= halfWidth && FP.Abs(dir.z) <= halfHeight) {
                        results.Add(new SearchUnit() { Target = target, SqrDistance = dir.sqrMagnitude});
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithCross(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, FP halfWidth, FP halfHeight, TSVector center, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                TSVector extents = new TSVector(TSMath.Max(halfWidth, halfHeight), FP.Half, TSMath.Max(halfHeight, halfWidth));
                AABB searchAABB = new AABB(center - extents, center + extents);
                foreach(var node in targets.Query(searchAABB))
                {
                    LSUnit target = self.LSUnit(node.UserData);
                    if (target == null || target.DeadMark > 0) {
                        continue;
                    }
                    if (!target.GetComponent<TypeComponent>().IsType(res.Type)) {
                        continue;
                    }
                    TSVector dir = target.GetComponent<TransformComponent>().Position - center;
                    TSVector absDir = TSVector.Abs(dir);
                    if ((absDir.x <= halfWidth && absDir.z <= halfHeight) ||
                        (absDir.x <= halfHeight && absDir.z <= halfWidth)) {
                        results.Add(new SearchUnit() { Target = target, SqrDistance = dir.sqrMagnitude });
                    }
                }
            }
        }
        
        private static void GetAttackTargetsWithLine(this LSTargetsComponent self, TeamType teamFlag, TbSearchRow res, FP halfWidth, FP height, TSVector center, TSVector forward, TSVector up, List<SearchUnit> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                TSVector right = TSVector.Cross(up, forward).normalized;
                TSVector p0 = center - right * halfWidth;
                TSVector p1 = center + right * halfWidth;
                TSVector p2 = center - right * halfWidth + forward * height;
                TSVector p3 = center + right * halfWidth + forward * height;
                TSVector min = TSVector.Min(TSVector.Min(p0, p1), TSVector.Min(p2, p3));
                TSVector max = TSVector.Max(TSVector.Max(p0, p1), TSVector.Max(p2, p3));
                AABB searchAABB = new AABB(new TSVector(min.x, -FP.One, min.z), new TSVector(max.x, FP.One, max.z) );
                foreach(var node in targets.Query(searchAABB))
                {
                    LSUnit target = self.LSUnit(node.UserData);
                    if (target == null || target.DeadMark > 0) {
                        continue;
                    }
                    if (!target.GetComponent<TypeComponent>().IsType(res.Type)) {
                        continue;
                    }
                    TSVector dir = target.GetComponent<TransformComponent>().Position - center;
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
                return targets.NodeCount;
            return 0;
        }

        public static void GetAttackTargets(this LSTargetsComponent self, TeamType teamFlag, EUnitType type, TSBounds bounds, List<long> results)
        {
            if (self.TeamLSUnitsMap.TryGetValue(teamFlag, out var targets))
            {
                AABB searchAABB = new AABB(bounds.min, bounds.max);
                foreach(var node in targets.Query(searchAABB))
                {
                    LSUnit target = self.LSUnit(node.UserData);
                    if (target == null || target.DeadMark > 0) {
                        continue;
                    }
                    if (!target.GetComponent<TypeComponent>().IsType(type))
                        continue;
                    TSVector point = target.GetComponent<TransformComponent>().Position;
                    TSVector min = bounds.min;
                    TSVector max = bounds.max;
                    if (min.x <= point.x && max.x >= point.x && min.z <= point.z && max.z >= point.z) {
                        results.Add(target.Id);
                    }
                }
            }
        }
        
    }
}