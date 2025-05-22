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
                if (owner.GetComponent<BeHitComponent>() != null) {
                    results.Add(new SearchUnit() { Target = owner });
                }
                return;
            }
            
            FP range = res.Range * FP.EN4;
            TSVector center = TSVector.zero;
            TeamType teamSelf = TeamType.None;
            
            TeamComponent teamComponent = owner.GetComponent<TeamComponent>();
            if (teamComponent != null)
            { 
                center = owner.GetComponent<TransformComponent>().Position;
                teamSelf = teamComponent.GetTeamType();
            }
            
            LSTargetsComponent lsTargetsComponent = owner.LSWorld().GetComponent<LSTargetsComponent>();
            switch (res.Team) {
                case ESearchTargetTeam.All:
                    lsTargetsComponent.GetAllAttackTargets(results);
                    break;
                case ESearchTargetTeam.Self:
                    lsTargetsComponent.GetAttackTargets(teamSelf, center, range, results);
                    break;
                case ESearchTargetTeam.Enemy:
                    for (TeamType i = TeamType.None; i < TeamType.Max; i++) {
                        if (i != teamSelf)
                            lsTargetsComponent.GetAttackTargets(i, center, range, results);
                    }
                    break;
                case ESearchTargetTeam.NotEnemy:
                    lsTargetsComponent.GetAttackTargets(teamSelf, center, range, results);
                    lsTargetsComponent.GetAttackTargets(TeamType.None, center, range, results);
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
            var dir = owner.GetComponent<TransformComponent>().Forward;
            var center = owner.GetComponent<TransformComponent>().Position;
            for (int idx = results.Count - 1; idx >= 0; idx--) {
                if (TSVector.Dot(dir, results[idx].Target.GetComponent<TransformComponent>().Position - center) < 0) {
                    results.RemoveAt(idx);
                }
            }
        }
        
        private static void FilterCount(int count, IList<SearchUnit> results)
        {
            for (int idx = results.Count - 1; idx >= count; idx--) {
                results.RemoveAt(idx);
            }
        }

        private static void FilterWithType(EUnitType type, IList<SearchUnit> results)
        {
            if (type == EUnitType.None) { return; }
            for (int idx = results.Count - 1; idx >= 0; idx--)
            {
                TypeComponent typeComponent = results[idx].Target.GetComponent<TypeComponent>();
                if (typeComponent == null || (typeComponent.GetUnitType() & type) == 0) {
                    results.RemoveAt(idx);
                }
            }
        }

        private static void FilterWithTableId(int[] ids, IList<SearchUnit> results)
        {
            if (ids.Length == 0) { return; }
            for (int idx = results.Count - 1; idx >= 0; idx--)
            {
                LSUnit target = results[idx].Target;
                int i = 0;
                for (; i < ids.Length; i++)
                {
                    if (ids[i] == target.TableId)
                        break;
                }
                if (i == ids.Length)
                {
                    results.RemoveAt(idx);
                }
            }
        }
        
        private static void FilterWithPriority(ESearchTargetPriority priority, List<SearchUnit> results)
        {
            switch (priority) {
                case ESearchTargetPriority.MaxDistance:
                    results.Sort((x, y) => -x.Distance.CompareTo(y.Distance));
                    break;
                case ESearchTargetPriority.MinDistance:
                    results.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                    break;
            }
        }
    }
}