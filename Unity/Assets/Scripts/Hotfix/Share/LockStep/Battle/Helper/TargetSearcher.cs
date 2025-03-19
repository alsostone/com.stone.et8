using System;
using System.Collections.Generic;

namespace ET
{
    public static class TargetSearcher
    {
        public static void Search(int id, LSUnit owner, List<SearchUnit> results)
        {
            // ResSearchTarget res = Table.Instance.GetResSearchTarget(id);
            // if (res == null) { return; }
            //
            // if (res.team == kSearchTargetTeam.SingleSelf) {
            //     // TODO if (owner.ComBeHit != null)
            //     {
            //         var distance = BattleClassPoolMgr.Instance.Get<EntityDistance>();
            //         distance.Target = owner;
            //         results.Add(distance);
            //     }
            //     return;
            // }
            //
            // var range = res.range * FP.EN4;
            // var center = FPVector.zero;
            // if (owner.ComTeam != null)
            // { 
            //     center = owner.ComTransform.Position;
            // }
            // var teamSelf = TeamFlag.None;
            // if (owner.ComTeam != null)
            // { 
            //     teamSelf = owner.ComTeam.TeamFlag;
            // }
            //
            // switch (res.team) {
            //     case kSearchTargetTeam.All:
            //         BattleWorld.Instance.EntityMgr.GetAllAttackTargets(results);
            //         break;
            //     case kSearchTargetTeam.Self:
            //         BattleWorld.Instance.EntityMgr.GetAttackTargets(teamSelf, center, range, results);
            //         break;
            //     case kSearchTargetTeam.Enemy:
            //         for (var i = TeamFlag.None; i < TeamFlag.Max; i++) {
            //             if (i != teamSelf)
            //                 BattleWorld.Instance.EntityMgr.GetAttackTargets(i, center, range, results);
            //         }
            //         break;
            //     case kSearchTargetTeam.NotEnemy:
            //         BattleWorld.Instance.EntityMgr.GetAttackTargets(teamSelf, center, range, results);
            //         BattleWorld.Instance.EntityMgr.GetAttackTargets(TeamFlag.None, center, range, results);
            //         break;
            //     case kSearchTargetTeam.Counter:
            //         owner.ComBeHit?.GetCounterAttack(center, range * range, results);
            //         break;
            //     default: throw new ArgumentOutOfRangeException();
            // }
            //
            // //FilterDirection(results, owner);
            // FilterWithType(res.type, results);
            // FilterWithTableId(res.table_id, results);
            // FilterWithPriority(res.priority, results);
            // FilterCount(res.count, results);
        }
        
        private static void FilterDirection(List<Entity> results, Entity owner)
        {
            // var dir = owner.ComTransform.Forward;
            // var center = owner.ComTransform.Position;
            // for (var idx = results.Count - 1; idx >= 0; idx--) {
            //     if (FPVector.Dot(dir, results[idx].ComTransform.Position - center) < 0) {
            //         BattleClassPoolMgr.Instance.Return(results[idx]);
            //         results.RemoveAt(idx);
            //     }
            // }
        }
        
        private static void FilterCount(int count, List<SearchUnit> results)
        {
            for (var idx = results.Count - 1; idx >= count; idx--) {
                ObjectPool.Instance.Recycle(results[idx]);
                results.RemoveAt(idx);
            }
        }

        public static void FilterWithType(int type, List<SearchUnit> results)
        {
            // if (type == 0) { return; }
            // for (var idx = results.Count - 1; idx >= 0; idx--) {
            //     if (((int)results[idx].Target.ComType.Type & type) == 0){
            //         ObjectPool.Instance.Recycle(results[idx]);
            //         results.RemoveAt(idx);
            //     }
            // }
        }

        private static void FilterWithTableId(List<int> ids, List<SearchUnit> results)
        {
            if (ids.Count == 0) { return; }
            for (var idx = results.Count - 1; idx >= 0; idx--) {
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
        
        public static void FilterWithPriority(ESearchTargetPriority priority, List<SearchUnit> results)
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

    }
}