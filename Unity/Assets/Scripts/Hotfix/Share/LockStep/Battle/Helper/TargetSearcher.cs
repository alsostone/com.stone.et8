using System;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [FriendOf(typeof(TypeComponent))]
    [FriendOf(typeof(TeamComponent))]
    public static class TargetSearcher
    {
        public static void Search(int id, LSUnit owner, List<SearchUnit> results)
        {
            TSVector center = TSVector.zero;
            TSVector forward = TSVector.forward;
            TSVector up = TSVector.up;
            TransformComponent transformComponent = owner.GetComponent<TransformComponent>();
            if (transformComponent != null) {
                center = transformComponent.Position;
                forward = transformComponent.Forward;
                up = transformComponent.Upwards;
            }
            Search(id, owner, center, forward, up, results);
        }
        
        public static FP Search(int id, LSUnit owner, TSVector center, TSVector forward, TSVector up, List<SearchUnit> results)
        {
            TbSearchRow res = TbSearch.Instance.Get(id);
            if (res == null) { return 0; }

            switch (res.Team)
            {
                case ESearchTargetTeam.Self:
                    if (owner.GetComponent<BeHitComponent>() != null) {
                        results.Add(new SearchUnit() { Target = owner });
                    }
                    return 0;
                case ESearchTargetTeam.Global:
                    results.Add(new SearchUnit() { Target = owner.LSUnit(LSConstValue.GlobalIdOffset)});
                    return 0;
                case ESearchTargetTeam.FriendTeam: {
                    TeamType team = owner.GetComponent<TeamComponent>()?.GetFriendTeam() ?? TeamType.None;
                    results.Add(new SearchUnit() { Target = owner.LSTeamUnit(team) });
                    return 0;
                }
                case ESearchTargetTeam.EnemyTeam: {
                    TeamType team = owner.GetComponent<TeamComponent>()?.GetEnemyTeam() ?? TeamType.None;
                    results.Add(new SearchUnit() { Target = owner.LSTeamUnit(team) });
                    return 0;
                }
            }
            
            LSTargetsComponent lsTargetsComponent = owner.LSWorld().GetComponent<LSTargetsComponent>();
            FP range = GetSearchRange(res, owner);
            
            switch (res.Team) {
                case ESearchTargetTeam.All: {
                    lsTargetsComponent.GetAllAttackTargets(results);
                    break;
                }
                case ESearchTargetTeam.Friend: {
                    TeamType team = owner.GetComponent<TeamComponent>().GetFriendTeam();
                    lsTargetsComponent.GetAttackTargetsWithShape(team, res, center, forward, up, range, results);
                    break;
                }
                case ESearchTargetTeam.FriendExcludeSelf: {
                    TeamType team = owner.GetComponent<TeamComponent>().GetFriendTeam();
                    lsTargetsComponent.GetAttackTargetsWithShape(team, res, center, forward, up, range, results);
                    for (int i = 0; i < results.Count; i++) {   // 排除自己的情形少 不要把判断放到获取目标的方法里去
                        if (results[i].Target == owner) {
                            results.RemoveAt(i);
                        }
                    }
                    break;
                }
                case ESearchTargetTeam.Enemy: {
                    TeamType team = owner.GetComponent<TeamComponent>().GetEnemyTeam();
                    lsTargetsComponent.GetAttackTargetsWithShape(team, res, center, forward, up, range, results);
                    break;
                }
                case ESearchTargetTeam.Counter: {
                    owner.GetComponent<BeHitComponent>()?.GetCounterAttack(center, range, results);
                    break;
                }
                default: throw new ArgumentOutOfRangeException();
            }
            
            // 形状过滤不放在此处 获取目标时考虑形状更高效
            FilterWithType(res.Type, results);
            FilterWithTableId(res.TableId, results);

            // 配置规则：数量未配置或<=0认为不限制数量
            if (res.Count > 0) {
                int targetCount = res.Count;
                if (res.AddCountProp != NumericType.None) {
                    PropComponent propComponent = owner.GetComponent<PropComponent>();
                    if (propComponent != null) {
                        int addCount = propComponent.Get(res.AddCountProp).AsInt();
                        targetCount += addCount;
                    }
                }
                FilterWithPriority(owner.GetRandom(), res.Priority, results);
                FilterCount(targetCount, results);
            }

            return range;
        }
        
        private static FP GetSearchRange(TbSearchRow res, LSUnit owner)
        {
            FP range = 0;
            PropComponent propComponent = owner.GetComponent<PropComponent>();
            if (res.ShapeParam.Length > 0 && res.ShapeParam[0] > 0)
            {
                range += res.ShapeParam[0] * FP.EN4;
                range += propComponent?.Radius ?? FP.Zero;
            }
            else if (propComponent != null)
            {
                range += propComponent.Get(NumericType.AtkRange);
                range += propComponent.Radius;
            }
            else
            {
                Log.Error($"TargetSearcher[{res.Id}] cannot find PropComponent");
            }
            return range;
        }
        
        private static void FilterCount(int count, IList<SearchUnit> results)
        {
            if (results.Count <= count) { return; }
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
                if (typeComponent == null || (typeComponent.Type & type) == 0) {
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
        
        private static void FilterWithPriority(TSRandom random, ESearchTargetPriority priority, List<SearchUnit> results)
        {
            switch (priority) {
                case ESearchTargetPriority.MaxDistance:
                    results.Sort((x, y) => -x.Distance.CompareTo(y.Distance));
                    break;
                case ESearchTargetPriority.MinDistance:
                    results.Sort((x, y) => x.Distance.CompareTo(y.Distance));
                    break;
                case ESearchTargetPriority.Random:
                    results.Shuffle(random);
                    break;
            }
        }
        
    }
}