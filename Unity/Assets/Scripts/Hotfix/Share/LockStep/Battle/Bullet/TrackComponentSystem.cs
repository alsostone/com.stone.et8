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
        {self.LSRoom()?.ProcessLog.LogFunction(47, self.LSParent().Id, trackId, target.Id);
            self.TrackId = trackId;
            self.HorSpeed = (FP)self.TbTrackRow.HorSpeed / LSConstValue.PropValueScale;
            if (target == null) {
                self.Target = 0;
                self.TargetPosition = targetPosition;
            } else {
                self.Target = target.Id;
                self.TargetPosition = target.GetComponent<TransformComponent>().TransformPoint(new TSVector(0, 2, 0));
            }
            
            switch (self.TbTrackRow.TowardType)
            {
                case ETrackTowardType.Target:
                case ETrackTowardType.Position:
                {
                    TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
                    self.CasterPosition = transformComponent.TransformPoint(new TSVector(0, 2, 0));
            
                    // 起止点的中心叠加高度为控制点
                    TSVector dir = self.TargetPosition - self.CasterPosition;
                    FP factor = (FP)self.TbTrackRow.ControlFactor / LSConstValue.PropValueScale;
                    FP height = (FP)self.TbTrackRow.ControlHeight / LSConstValue.PropValueScale;
                    TSVector control = dir * factor + transformComponent.TransformDirection(new TSVector(0, height, 0));
                    
                    self.ControlPosition = self.CasterPosition + control;
                    self.Duration = dir.magnitude / self.HorSpeed;
                    break;
                }
            }
            self.Tick();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this TrackComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(46, self.LSParent().Id);
            self.Tick();
        }
        
        private static void Tick(this TrackComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(45, self.LSParent().Id);
            self.EclipseTime += (FP)LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            
            TransformComponent ownerTransform = self.LSOwner().GetComponent<TransformComponent>();
            TSVector position = ownerTransform.Position;
            switch (self.TbTrackRow.TowardType)
            {
                case ETrackTowardType.Target:
                {
                    // 防止目标死亡导致取不到目标位置
                    LSUnit target = self.LSUnit(self.Target);
                    if (target != null)
                        self.TargetPosition = target.GetComponent<TransformComponent>().TransformPoint(new TSVector(0, 2, 0));
                    ownerTransform.Position = TSBezier.GetPoint(self.CasterPosition, self.ControlPosition, self.TargetPosition, self.EclipseTime / self.Duration);
                    break;
                }
                case ETrackTowardType.Direction:
                {
                    // 指向固定方向时 抛物线效果不生效
                    TSVector forward = self.TargetPosition;
                    TSVector offset = forward.normalized * self.HorSpeed * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
                    ownerTransform.Position += offset;
                    break;
                }
                case ETrackTowardType.Position:
                {
                    ownerTransform.Position = TSBezier.GetPoint(self.CasterPosition, self.ControlPosition, self.TargetPosition, self.EclipseTime / self.Duration);
                    break;
                }
            }
            ownerTransform.Forward = ownerTransform.Position - position;
        }
        
    }
}