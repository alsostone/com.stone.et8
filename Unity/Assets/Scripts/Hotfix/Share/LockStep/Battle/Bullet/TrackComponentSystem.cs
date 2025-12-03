using System;
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
        private static void Awake(this TrackComponent self, int trackId, LSUnit target)
        {
            self.TrackId = trackId;
            self.Target = target.Id;
            self.HorSpeed = (FP)self.TbTrackRow.HorSpeed / LSConstValue.PropValueScale;
            
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            self.CasterPosition = transformComponent.Position;  // 子弹的位置就是起始点 而位置由释放者决定

            switch (self.TbTrackRow.TowardType)
            {
                case ETrackTowardType.Target:
                {
                    self.TargetPosition = target.GetComponent<TransformComponent>().GetAttachPoint(AttachPoint.Chest);
                    self.ControlPosition = self.CalcControlPosition(transformComponent);
                    break;
                }
                case ETrackTowardType.Direction:
                {
                    self.TargetPosition = (self.TargetPosition - self.CasterPosition).normalized;
                    break;
                }
                case ETrackTowardType.Position:
                {
                    self.TargetPosition = target.GetComponent<TransformComponent>().Position;
                    self.ControlPosition = self.CalcControlPosition(transformComponent);
                    break;
                }
            }

            self.Duration = TSVector.Distance(self.TargetPosition, self.CasterPosition) / self.HorSpeed;
            self.Tick();
        }

        [EntitySystem]
        private static void Awake(this TrackComponent self, int trackId, TSVector targetPosition)
        {
            self.TrackId = trackId;
            self.Target = 0;
            self.HorSpeed = (FP)self.TbTrackRow.HorSpeed / LSConstValue.PropValueScale;
            self.TargetPosition = targetPosition;
            
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            self.CasterPosition = transformComponent.Position;  // 子弹的位置就是起始点 而位置由释放者决定
            self.Duration = TSVector.Distance(self.TargetPosition, self.CasterPosition) / self.HorSpeed;

            switch (self.TbTrackRow.TowardType)
            {
                case ETrackTowardType.Direction:
                {
                    self.TargetPosition = (self.TargetPosition - self.CasterPosition).normalized;
                    break;
                }
                case ETrackTowardType.Target:
                case ETrackTowardType.Position:
                {
                    self.ControlPosition = self.CalcControlPosition(transformComponent);
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
        
        private static TSVector CalcControlPosition(this TrackComponent self, TransformComponent ownerTransform)
        {
            // 起止点的中心叠加高度为控制点
            TSVector dir = self.TargetPosition - ownerTransform.Position;
            FP factor = (FP)self.TbTrackRow.ControlFactor / LSConstValue.PropValueScale;
            FP height = (FP)self.TbTrackRow.ControlHeight / LSConstValue.PropValueScale;
            TSVector control = dir * factor + ownerTransform.TransformDirection(new TSVector(0, height, 0));
            return ownerTransform.Position + control;
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
                        self.TargetPosition = target.GetComponent<TransformComponent>().GetAttachPoint(AttachPoint.Chest);
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