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
            
            if (lsUnit.GetComponent<FlagComponent>().HasRestrict(FlagRestrict.NotAIAlert))
            {
                this.Blackboard.SetBool(AIConstValue.IsStateOfAlert, false);
            }
            else
            {
                List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
                TargetSearcher.Search(AIConstValue.AwarenessSearchEnemy, lsUnit, targets);
                if (targets.Count > 0)
                {
                    lsUnit.GetComponent<MoveFlowFieldComponent>()?.Stop();
                    lsUnit.GetComponent<MovePathFindingComponent>()?.Stop();
                    this.Blackboard.SetBool(AIConstValue.IsStateOfAlert, true);
                    this.Blackboard.SetInt(AIConstValue.HasEnemyCount, targets.Count);
                    this.Blackboard.SetLong(AIConstValue.HasEnemyEntityId, targets[0].Target.Id);
                }
                else
                {
                    this.Blackboard.SetBool(AIConstValue.IsStateOfAlert, false);
                }
                targets.Clear();
                ObjectPool.Instance.Recycle(targets);
            }
        }
    }
}