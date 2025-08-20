using System.Collections.Generic;
using MemoryPack;
using NPBehave;
using TrueSync;

namespace ET
{
    [AINode]
    [MemoryPackable]
    public partial class ServiceAwareness : Service
    {
        public ServiceAwareness(FP interval, FP randomVariation, Node decoratee) : base(interval, randomVariation, decoratee)
        {
        }

        protected override void OnService()
        {
            LSUnit lsUnit = Agent as LSUnit;
            
            List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
            TargetSearcher.Search(AIConstValue.AwarenessSearchEnemy, lsUnit, targets);
            this.Blackboard.SetBool(AIConstValue.HasEnemy, targets.Count > 0);
            this.Blackboard.SetInt(AIConstValue.HasEnemyCount, targets.Count);
            this.Blackboard.SetLong(AIConstValue.HasEnemyEntityId, targets.Count > 0 ? targets[0].Target.Id : 0);
            targets.Clear();
            ObjectPool.Instance.Recycle(targets);
        }
    }
}