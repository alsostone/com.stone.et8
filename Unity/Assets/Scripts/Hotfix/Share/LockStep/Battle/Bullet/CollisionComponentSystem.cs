
namespace ET
{
    [LSEntitySystemOf(typeof(CollisionComponent))]
    [EntitySystemOf(typeof(CollisionComponent))]
    [FriendOf(typeof(CollisionComponent))]
    public static partial class CollisionComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CollisionComponent self, int searchId, int interval)
        {
            self.SearchID = searchId;
            self.TestingInterval = interval;
            self.TestingFrame = self.LSWorld().Frame + interval;
            
            LSUnit lsOwner = self.LSOwner();
            TransformComponent transformComponent = lsOwner.GetComponent<TransformComponent>();
            self.PreviousPosition = transformComponent.Position;
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this CollisionComponent self)
        {
            int frame = self.LSWorld().Frame;
            if (frame < self.TestingFrame)
                return;
            self.TestingFrame = frame + self.TestingInterval;
            
            LSUnit lsUnit = self.LSOwner();
            
            
            // // 创建子弹时把目标搜索出来 子弹决定命中时机（通过距离判定，非碰撞检测）
            // List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
            // FP range = TargetSearcher.Search(searchId, target, thisTransform.Position, thisTransform.Forward, thisTransform.Upwards, targets);
            // targets.Sort((x, y) => x.SqrDistance.CompareTo(y.SqrDistance));
            // lsUnit.AddComponent<TrackComponent, int, int, int, FP>(row.HorSpeed, row.ControlFactor, row.ControlHeight, range);
            // lsUnit.AddComponent<BulletComponent, int, LSUnit, List<SearchUnit>>(bulletId, caster, targets);
            // targets.Clear();
            // ObjectPool.Instance.Recycle(targets);

        }
        
    }
}