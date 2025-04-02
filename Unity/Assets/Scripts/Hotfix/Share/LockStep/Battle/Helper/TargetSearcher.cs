using System;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    public static class TargetSearcher
    {
        public static void Search(int id, LSUnit owner, List<SearchUnit> results)
        {
            TbSearchRow res = TbSearch.Instance.Get(id);
            if (res == null) { return; }
            
            if (res.Team == ESearchTargetTeam.SingleSelf) {
                if (owner.GetComponent<BeHitComponent>() != null)
                {
                    SearchUnit distance = ObjectPool.Instance.Fetch<SearchUnit>();
                    distance.Target = owner;
                    results.Add(distance);
                }
                return;
            }
            
            FP range = res.Range * FP.EN4;
            TSVector center = TSVector.zero;
            TeamType teamSelf = TeamType.None;
            
            TeamComponent teamComponent = owner.GetComponent<TeamComponent>();
            if (teamComponent != null)
            { 
                center = owner.Position;
                teamSelf = teamComponent.GetTeamType();
            }
            
            switch (res.Team) {
                case ESearchTargetTeam.All:
                    owner.LSWorld().GetAllAttackTargets(results);
                    break;
                case ESearchTargetTeam.Self:
                    owner.LSWorld().GetAttackTargets(teamSelf, center, range, results);
                    break;
                case ESearchTargetTeam.Enemy:
                    for (TeamType i = TeamType.None; i < TeamType.Max; i++) {
                        if (i != teamSelf)
                            owner.LSWorld().GetAttackTargets(i, center, range, results);
                    }
                    break;
                case ESearchTargetTeam.NotEnemy:
                    owner.LSWorld().GetAttackTargets(teamSelf, center, range, results);
                    owner.LSWorld().GetAttackTargets(TeamType.None, center, range, results);
                    break;
                case ESearchTargetTeam.Counter:
                    owner.GetComponent<BeHitComponent>()?.GetCounterAttack(center, range * range, results);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
            
            FilterDirection(results, owner);
            FilterWithType(res.Type, results);
            FilterWithTableId(res.TableId, results);
            FilterWithPriority(res.Priority, results);
            FilterCount(res.Count, results);
        }
        
        private static void FilterDirection(IList<SearchUnit> results, LSUnit owner)
        {
            var dir = owner.Forward;
            var center = owner.Position;
            for (int idx = results.Count - 1; idx >= 0; idx--) {
                if (TSVector.Dot(dir, results[idx].Target.Position - center) < 0) {
                    ObjectPool.Instance.Recycle(results[idx]);
                    results.RemoveAt(idx);
                }
            }
        }
        
        private static void FilterCount(int count, IList<SearchUnit> results)
        {
            for (int idx = results.Count - 1; idx >= count; idx--) {
                ObjectPool.Instance.Recycle(results[idx]);
                results.RemoveAt(idx);
            }
        }

        private static void FilterWithType(EUnitType type, IList<SearchUnit> results)
        {
            if (type == EUnitType.None) { return; }
            for (int idx = results.Count - 1; idx >= 0; idx--)
            {
                TypeComponent typeComponent = results[idx].Target.GetComponent<TypeComponent>();
                if (typeComponent == null || (typeComponent.GetUnitType() & type) == 0){
                    ObjectPool.Instance.Recycle(results[idx]);
                    results.RemoveAt(idx);
                }
            }
        }

        private static void FilterWithTableId(IReadOnlyCollection<int> ids, IList<SearchUnit> results)
        {
            if (ids.Count == 0) { return; }
            for (int idx = results.Count - 1; idx >= 0; idx--) {
                var target = results[idx].Target;
                // switch (target.ComType.Type) {
                //     case EntityType.Building:
                //         if (ids.IndexOf(target.ComBuilding.ResBuilding.id_key) != -1) {
                //             BattleClassPoolMgr.Instance.Return(results[idx]);
                //             results.RemoveAt(idx);
                //         }
                //         break;
                //     case EntityType.Soldier:
                //         if (ids.IndexOf(target.ComSoldier.ResSoldier.id_key) != -1) {
                //             BattleClassPoolMgr.Instance.Return(results[idx]);
                //             results.RemoveAt(idx);
                //         }
                //         break;
                //     case EntityType.Plant:
                //         if (ids.IndexOf(target.ComPlant.ResPlant.id_key) != -1) {
                //             BattleClassPoolMgr.Instance.Return(results[idx]);
                //             results.RemoveAt(idx);
                //         }
                //         break;
                //     default: break;
                // }
            }
        }
        
        private static void FilterWithPriority(ESearchTargetPriority priority, List<SearchUnit> results)
        {
            switch (priority) {
                case ESearchTargetPriority.MAX_DISTANCE:
                    results.Sort((x, y) => -x.Distance.CompareTo(y.Distance));
                    break;
                case ESearchTargetPriority.MIN_DISTANCE:
                    results.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                    break;
            }
        }
        
        private static void GetAllAttackTargets(this LSWorld world, List<SearchUnit> results)
        {
            // for (TeamType i = 0; i <= TeamType.TeamB; i++)
            // {
            //     if (mTeamPropertyMapping.TryGetValue((TeamFlag)i, out var targets)) {
            //         foreach (var target in targets) {
            //             if (target.ComActive.Active) {
            //                 var distance = BattleClassPoolMgr.Instance.Get<EntityDistance>();
            //                 distance.Target = target;
            //                 results.Add(distance);
            //             }
            //         }
            //     }
            // }
        }
        
        private static void GetAttackTargets(this LSWorld world, TeamType teamFlag, TSVector center, FP range, List<SearchUnit> results)
        {
            // if (mTeamPropertyMapping.TryGetValue(teamFlag, out var targets)) {
            //     foreach (var target in targets) {
            //         if (target.ComActive.Active) {
            //             var dis = (target.ComTransform.Position - center).sqrMagnitude;
            //             var sqrRange = GetAttackSqrRange(target, range);
            //             if (sqrRange >= dis) {
            //                 var distance = BattleClassPoolMgr.Instance.Get<EntityDistance>();
            //                 distance.Target = target;
            //                 distance.Distance = dis;
            //                 results.Add(distance);
            //             }
            //         }
            //     }
            // }
        }
    }
}