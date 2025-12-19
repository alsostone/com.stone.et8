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
        private static void Awake(this LSViewTransformComponent self, Transform transform, AttachPointCollector collector, bool uesViewRotation)
        {
            self.Transform = transform;
            self.AttachPointCollector = collector;
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
            
            Vector3 postion = self.Transform.position;
            self.Transform.position = Vector3.SmoothDamp(postion, self.Position, ref self.CurrentVelocity, .125f);
            
            // 为使得表现平滑，旋转不敏感的单位 表现层自己计算旋转
            // 如子弹 与逻辑旋转不严格一致并不会带来问题，平滑更重要
            if (self.IsUesViewRotation)
            {
                Vector3 dir = postion - self.Transform.position;
                if (dir.sqrMagnitude > 0.0001f) {
                    self.Transform.forward = dir;
                }
            }
            else
            {
                Quaternion rotation = self.Transform.rotation;
                float angle = Mathf.Abs(Quaternion.Angle(rotation, self.Rotation));
                if (angle > 0.1f) {
                    float time = 1.0f / 720 * angle;
                    self.Transform.rotation = Quaternion.Lerp(rotation, self.Rotation, Time.deltaTime / time);
                }
            }
        }
        
        public static void SetPosition(this LSViewTransformComponent self, Vector3 position, bool immediate = false)
        {
            self.Position = position;
            if (immediate) {
                self.Transform.position = position;
            }
        }
        
        public static void SetRotation(this LSViewTransformComponent self, Quaternion rotation, bool immediate = false)
        {
            self.Rotation = rotation;
            if (immediate) {
                self.Transform.rotation = rotation;
            }
        }
        
        private static void ResetTransfrom(this LSViewTransformComponent self)
        {
            if (!self.Enabled) { return; }
            
            LSUnit lsUnit = self.LSViewOwner().GetUnit();
            TransformComponent transformComponent = lsUnit.GetComponent<TransformComponent>();
            self.SetPosition(transformComponent.Position.ToVector(), true);
            self.SetRotation(transformComponent.Rotation.ToQuaternion(), true);
        }
        
        public static void SetTransformEnabled(this LSViewTransformComponent self, bool enabled)
        {
            self.Enabled = enabled;
        }
        
        public static Vector3 GetAttachPoint(this LSViewTransformComponent self, AttachPoint attachPoint)
        {
            if (self.AttachPointCollector != null)
            {
                return self.AttachPointCollector.GetAttachPoint(attachPoint).position;
            }
            return self.Transform.position;
        }

        public static Transform GetAttachTransform(this LSViewTransformComponent self, AttachPoint attachPoint)
        {
            if (self.AttachPointCollector != null)
            {
                return self.AttachPointCollector.GetAttachPoint(attachPoint);
            }
            return self.Transform;
        }
    }
}