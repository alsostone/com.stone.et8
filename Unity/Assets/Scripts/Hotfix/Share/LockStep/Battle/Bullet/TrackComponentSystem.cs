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
            
            self.StartFrame = self.LSWorld().Frame;
            self.HorSpeed = self.TbTrackRow.HorSpeed * FP.EN4;

            TransformComponent targetTransform = target.GetComponent<TransformComponent>();
            self.TargetPostion = targetTransform.Position;
            
            // 根据高度差计算竖直加速度和初速度
            if (self.TbTrackRow.VerSpeed > 0)
            {
                TransformComponent ownerTransform = self.LSOwner().GetComponent<TransformComponent>();
                FP distance = (self.TargetPostion - ownerTransform.Position).magnitude;
                int frameCountHalf = (int)(distance / self.HorSpeed * LSConstValue.FrameCountPerSecond / 2);
                self.VerAcceleration = (self.TbTrackRow.VerSpeed * FP.EN4) / (frameCountHalf * (frameCountHalf + 1) / 2);
                self.VerSpeed = self.VerAcceleration * frameCountHalf;
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
            TransformComponent ownerTransform = self.LSOwner().GetComponent<TransformComponent>();
            TransformComponent targetTransform = self.LSUnit(self.Target).GetComponent<TransformComponent>();
            TSVector forwardHor = targetTransform.Position - ownerTransform.Position;
            forwardHor.y = 0;
            TSVector offsetHor = forwardHor.normalized * self.HorSpeed * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            
            TSVector position = ownerTransform.Position;
            ownerTransform.Position = new TSVector(position.x + offsetHor.x, position.y + self.VerSpeed, position.z + offsetHor.z);
            ownerTransform.Forward = ownerTransform.Position - position;
            self.VerSpeed -= self.VerAcceleration;

            
            //
            // ownerTransform.Move(forward);
            //
            // TSVector2 distance = targetTransform.Position - ownerTransform.Position;
            // if (distance.LengthSquared() < FP.EN4)
            // {
            //     self.OnReachTarget(true);
            // }
            //
            // var rotation = TSQuaternion.LookRotation(targetTransform.Position - ownerTransform.Position);
        }
        
        private static void OnReachTarget(this TrackComponent self, bool reach)
        {
            // if (reach) {
            //     LSUnit caster = self.LSUnit(self.Caster);
            //     LSUnit target = self.LSUnit(self.Target);
            //     EffectExecutor.Execute(self.TbBulletRow.EffectGroupId, caster, target, self.LSOwner());
            // }
            // EventSystem.Instance.Publish(self.LSWorld(), new LSUnitRemove() { Id = self.LSOwner().Id });
            // self.LSOwner().Dispose();
        }
    }
}