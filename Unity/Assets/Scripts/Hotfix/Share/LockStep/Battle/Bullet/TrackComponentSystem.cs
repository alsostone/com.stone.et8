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
        private static void Awake(this TrackComponent self, int speed, int controlFactor, int controlHeight, FP distance)
        {self.LSRoom()?.ProcessLog.LogFunction(170, self.LSParent().Id, speed, controlFactor, controlHeight, distance.V);
            self.HorSpeed = (FP)speed / LSConstValue.PropValueScale;
            self.TowardType = ETrackTowardType.Direction;
            
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            self.CasterPosition = transformComponent.Position;  // 子弹的位置就是起始点 而位置由释放者决定
            self.TargetPosition = self.CasterPosition + transformComponent.Forward * distance;
            
            // 起止点的中心叠加高度为控制点
            if (controlHeight > 0) {
                TSVector dir = self.TargetPosition - self.CasterPosition;
                FP factor = (FP)controlFactor / LSConstValue.PropValueScale;
                FP height = (FP)controlHeight / LSConstValue.PropValueScale;
                TSVector control = dir * factor + transformComponent.TransformDirection(new TSVector(0, height, 0));
                self.ControlPosition = self.CasterPosition + control;
                self.Duration = distance / self.HorSpeed;
                self.IsUesBezier = true;
            }
        }
        
        [EntitySystem]
        private static void Awake(this TrackComponent self, int speed, int controlFactor, int controlHeight, LSUnit target)
        {self.LSRoom()?.ProcessLog.LogFunction(169, self.LSParent().Id, speed, controlFactor, controlHeight, target.Id);
            self.Target = target.Id;
            
            self.HorSpeed = (FP)speed / LSConstValue.PropValueScale;
            self.TowardType = ETrackTowardType.Target;
            
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            self.CasterPosition = transformComponent.Position;  // 子弹的位置就是起始点 而位置由释放者决定
            self.TargetPosition = target.GetComponent<TransformComponent>().GetAttachPoint(AttachPoint.Chest);
            
            // 起止点的中心叠加高度为控制点
            if (controlHeight > 0) {
                TSVector dir = self.TargetPosition - self.CasterPosition;
                FP factor = (FP)controlFactor / LSConstValue.PropValueScale;
                FP height = (FP)controlHeight / LSConstValue.PropValueScale;
                TSVector control = dir * factor + transformComponent.TransformDirection(new TSVector(0, height, 0));
                self.ControlPosition = self.CasterPosition + control;
                self.Duration = dir.magnitude / self.HorSpeed;
                self.IsUesBezier = true;
            }
        }

        [EntitySystem]
        private static void Awake(this TrackComponent self, int speed, int controlFactor, int controlHeight, TSVector targetPosition)
        {self.LSRoom()?.ProcessLog.LogFunction(168, self.LSParent().Id, speed, controlFactor, controlHeight, targetPosition.x.V, targetPosition.y.V, targetPosition.z.V);
            self.HorSpeed = (FP)speed / LSConstValue.PropValueScale;
            self.TowardType = ETrackTowardType.Position;
            
            TransformComponent transformComponent = self.LSOwner().GetComponent<TransformComponent>();
            self.CasterPosition = transformComponent.Position;  // 子弹的位置就是起始点 而位置由释放者决定
            self.TargetPosition = targetPosition;
            
            // 起止点的中心叠加高度为控制点
            if (controlHeight > 0) {
                TSVector dir = self.TargetPosition - self.CasterPosition;
                FP factor = (FP)controlFactor / LSConstValue.PropValueScale;
                FP height = (FP)controlHeight / LSConstValue.PropValueScale;
                TSVector control = dir * factor + transformComponent.TransformDirection(new TSVector(0, height, 0));
                self.ControlPosition = self.CasterPosition + control;
                self.Duration = dir.magnitude / self.HorSpeed;
                self.IsUesBezier = true;
            }
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this TrackComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(46, self.LSParent().Id);
            self.Tick();
        }
        
        private static void Tick(this TrackComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(45, self.LSParent().Id);
            if (self.IsReached) { return; }
            self.ElapsedTime += self.LSWorld().DeltaTime;
            
            if (self.TowardType == ETrackTowardType.Target)
            {
                LSUnit target = self.LSUnit(self.Target);
                if (target != null && target.DeadMark == 0)
                    self.TargetPosition = target.GetComponent<TransformComponent>().GetAttachPoint(AttachPoint.Chest);
            }

            TransformComponent ownerTransform = self.LSOwner().GetComponent<TransformComponent>();
            TSVector position = ownerTransform.Position;
            if (self.IsUesBezier)
            {
                if (self.ElapsedTime >= self.Duration) {
                    ownerTransform.SetPosition(self.TargetPosition);
                    self.IsReached = true;
                } else {
                    ownerTransform.SetPosition(TSBezier.GetPoint(self.CasterPosition, self.ControlPosition, self.TargetPosition, self.ElapsedTime / self.Duration));
                }
            }
            else
            {
                TSVector dir = self.TargetPosition - position;
                FP distanceThisFrame = self.HorSpeed * self.LSWorld().DeltaTime;
                if (dir.sqrMagnitude <= distanceThisFrame * distanceThisFrame) {
                    ownerTransform.SetPosition(self.TargetPosition);
                    self.IsReached = true;
                } else {
                    ownerTransform.SetPosition(position + dir.normalized * distanceThisFrame);
                }
            }
        }
    }
}