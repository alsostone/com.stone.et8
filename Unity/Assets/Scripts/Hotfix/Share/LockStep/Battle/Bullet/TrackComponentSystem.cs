using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(TrackComponent))]
    [EntitySystemOf(typeof(TrackComponent))]
    [FriendOf(typeof(TrackComponent))]
    [FriendOf(typeof(TransformComponent))]
    public static partial class TrackComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TrackComponent self, int trackId, LSUnit target, TSVector targetPosition)
        {
            self.TrackId = trackId;
            self.HorSpeed = self.TbTrackRow.HorSpeed * FP.EN4;
            if (target == null) {
                self.Target = 0;
                self.TargetPostion = targetPosition;
            } else {
                self.Target = target.Id;
                self.TargetPostion = target.GetComponent<TransformComponent>().Position;
            }
            
            switch (self.TbTrackRow.TowardType)
            {
                case ETrackTowardType.Target:
                case ETrackTowardType.Position:
                {
                    TransformComponent ownerTransform = self.LSOwner().GetComponent<TransformComponent>();
                    self.CasterPosition = ownerTransform.Position;
            
                    // 起止点的中心叠加高度为控制点
                    TSVector dir = self.TargetPostion - self.CasterPosition;
                    self.ControlPosition = self.CasterPosition + dir * (self.TbTrackRow.ControlFactor * FP.EN4);
                    FP y = (self.TbTrackRow.ControlHeight * FP.EN4) + self.ControlPosition.y;
                    self.ControlPosition = new TSVector(self.ControlPosition.x, y, self.ControlPosition.z);
                    self.Duration = dir.magnitude / self.HorSpeed;
                    break;
                }
            }
            self.Tick();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this TrackComponent self)
        {
            self.Tick();
        }
        
        private static void Tick(this TrackComponent self)
        {
            self.EclipseTime += (FP)LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            
            TransformComponent ownerTransform = self.LSOwner().GetComponent<TransformComponent>();
            TSVector position = ownerTransform.Position;
            switch (self.TbTrackRow.TowardType)
            {
                case ETrackTowardType.Target:
                {
                    TransformComponent targetTransform = self.LSUnit(self.Target).GetComponent<TransformComponent>();
                    ownerTransform.Position = TSBezier.GetPoint(self.CasterPosition, self.ControlPosition, targetTransform.Position, self.EclipseTime / self.Duration);
                    break;
                }
                case ETrackTowardType.Direction:
                {
                    // 指向固定方向时 抛物线效果不生效
                    TSVector forward = self.TargetPostion;
                    TSVector offset = forward.normalized * self.HorSpeed * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
                    ownerTransform.Position += offset;
                    break;
                }
                case ETrackTowardType.Position:
                {
                    ownerTransform.Position = TSBezier.GetPoint(self.CasterPosition, self.ControlPosition, self.TargetPostion, self.EclipseTime / self.Duration);
                    break;
                }
            }
            ownerTransform.Forward = ownerTransform.Position - position;
        }
        
    }
}