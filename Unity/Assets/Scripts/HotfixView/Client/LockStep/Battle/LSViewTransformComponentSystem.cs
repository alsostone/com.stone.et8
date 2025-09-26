using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewTransformComponent))]
    [LSEntitySystemOf(typeof(LSViewTransformComponent))]
    [FriendOf(typeof(LSViewTransformComponent))]
    public static partial class LSViewTransformComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewTransformComponent self, Transform transform, bool uesViewRotation)
        {
            self.Transform = transform;
            self.IsUesViewRotation = uesViewRotation;
            self.ResetTransfrom();
        }
        
        [LSEntitySystem]
        private static void LSRollback(this LSViewTransformComponent self)
        {
            self.ResetTransfrom();
        }
        
        [EntitySystem]
        private static void Update(this LSViewTransformComponent self)
        {
            if (!self.Enabled) { return; }
            
            LSUnit unit = self.LSViewOwner().GetUnit();
            
            TransformComponent transformComponent = unit.GetComponent<TransformComponent>();
            self.Position = transformComponent.Position.ToVector();
            
            Vector3 postion = self.Transform.position;
            self.Transform.position = Vector3.SmoothDamp(postion, self.Position, ref self.CurrentVelocity, .125f);
            
            // 为使得表现平滑，旋转不敏感的单位 表现层自己计算旋转
            // 如子弹 与逻辑旋转不严格一致并不会带来严重问题，平滑更重要
            if (self.IsUesViewRotation)
            {
                Vector3 dir = postion - self.Transform.position;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    self.Transform.forward = postion - self.Transform.position;
                }
            }
            else
            {
                Quaternion rotationTo = transformComponent.Rotation.ToQuaternion();
                Quaternion rotation = self.Transform.rotation;
                float angle = Mathf.Abs(Quaternion.Angle(rotation, rotationTo));
                float time = 1.0f / 720 * angle;
                self.Transform.rotation = Quaternion.Lerp(rotation, rotationTo, Time.deltaTime / time);
            }
        }
        
        private static void ResetTransfrom(this LSViewTransformComponent self)
        {
            if (!self.Enabled) { return; }
            
            LSUnit unit = self.LSViewOwner().GetUnit();
            TransformComponent transformComponent = unit.GetComponent<TransformComponent>();
            self.Transform.position = transformComponent.Position.ToVector();
            self.Transform.rotation = transformComponent.Rotation.ToQuaternion();
        }
        
        public static void SetTransformEnabled(this LSViewTransformComponent self, bool enabled)
        {
            self.Enabled = enabled;
        }
        
        public static Transform GetAttachTransform(this LSViewTransformComponent self, EBindPointType attachPoint)
        {
            return self.Transform;
        }
    }
}