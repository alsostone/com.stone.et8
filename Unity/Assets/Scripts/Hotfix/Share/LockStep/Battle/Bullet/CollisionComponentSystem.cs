using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(CollisionComponent))]
    [FriendOf(typeof(CollisionComponent))]
    public static partial class CollisionComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CollisionComponent self, int interval)
        {
            self.TestingInterval = interval;
            self.TestingFrame = self.LSWorld().Frame + interval;
            
            LSUnit lsOwner = self.LSOwner();
            TransformComponent transformComponent = lsOwner.GetComponent<TransformComponent>();
            self.PreviousPosition = transformComponent.Position;
        }
        
        [EntitySystem]
        private static void Destroy(this CollisionComponent self)
        {
            self.HitSet.Clear();
        }
        
        public static void GetCollisionTargets(this CollisionComponent self, List<SearchUnit> targets, bool force = false)
        {
            int frame = self.LSWorld().Frame;
            if (!force && frame < self.TestingFrame)
                return;
            self.TestingFrame = frame + self.TestingInterval;
            
            LSUnit lsOwner = self.LSOwner();
            TeamComponent teamComponent = lsOwner.GetComponent<TeamComponent>();
            TransformComponent transformComponent = lsOwner.GetComponent<TransformComponent>();
            LSTargetsComponent lsTargetsComponent = self.LSWorld().GetComponent<LSTargetsComponent>();
            
            IList<TeamType> teams = teamComponent.GetEnemyTeams();
            foreach (TeamType team in teams) {
                lsTargetsComponent.GetAttackTargetsWithSegment(team, (EUnitType)(-1), self.PreviousPosition, transformComponent.Position, targets);     
            }
            teams.Clear();
            ObjectPool.Instance.Recycle(teams);
            
            // 剔除已命中过的目标
            if (targets.Count > 0) {
                for (int i = targets.Count - 1; i >= 0; i--) {
                    LSUnit target = targets[i].Target;
                    if (self.HitSet.Contains(target.Id))
                        targets.RemoveAt(i);
                    else
                        self.HitSet.Add(target.Id);
                }
            }
            self.PreviousPosition = transformComponent.Position;
        }
        
    }
}