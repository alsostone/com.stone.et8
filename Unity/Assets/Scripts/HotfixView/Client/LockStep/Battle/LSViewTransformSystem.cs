using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewTransformComponent))]
    [LSEntitySystemOf(typeof(LSViewTransformComponent))]
    [FriendOf(typeof(LSViewTransformComponent))]
    public static partial class LSViewTransformSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewTransformComponent self, Transform transform)
        {
            self.Transform = transform;
        }
        
        [LSEntitySystem]
        private static void LSRollback(this LSViewTransformComponent self)
        {
            LSUnit unit = self.LSViewOwner().GetUnit();
            
            TransformComponent transformComponent = unit.GetComponent<TransformComponent>();
            self.Transform.position = transformComponent.Position.ToVector();
            self.Transform.rotation = transformComponent.Rotation.ToQuaternion();
        }
        
        [EntitySystem]
        private static void Update(this LSViewTransformComponent self)
        {
            LSUnit unit = self.LSViewOwner().GetUnit();
            
            TransformComponent transformComponent = unit.GetComponent<TransformComponent>();
            self.Position = transformComponent.Position.ToVector();
            self.Rotation = transformComponent.Rotation.ToQuaternion();

            Quaternion rotation = self.Transform.rotation;
            float angle = Mathf.Abs(Quaternion.Angle(rotation, self.Rotation));
            self.Transform.rotation = Quaternion.Lerp(rotation, self.Rotation, Time.deltaTime / (angle / 720f));
            self.Transform.position = Vector3.SmoothDamp(self.Transform.position, self.Position, ref self.CurrentVelocity, .125f);
        }
        
    }
}