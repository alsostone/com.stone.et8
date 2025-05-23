using System;
using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSUnitView))]
    [LSEntitySystemOf(typeof(LSUnitView))]
    [FriendOf(typeof(LSUnitView))]
    public static partial class LSUnitViewSystem
    {
        [EntitySystem]
        private static void Awake(this LSUnitView self, GameObject go)
        {
            self.GameObject = go;
            self.Transform = go.transform;

        }
        
        [EntitySystem]
        private static void Destroy(this LSUnitView self)
        {
            if (self.GameObject != null)
            {
                UnityEngine.Object.Destroy(self.GameObject);
                self.GameObject = null;
            }
        }

        [LSEntitySystem]
        private static void LSRollback(this LSUnitView self)
        {
            //LSUnit unit = self.GetUnit();
            //self.Transform.position = unit.Position.ToVector();
            //self.Transform.rotation = unit.Rotation.ToQuaternion();
            //self.t = 0;
            //self.totalTime = 0;
        }

        [EntitySystem]
        private static void Update(this LSUnitView self)
        {
            LSUnit unit = self.GetUnit();
            TransformComponent transformComponent = unit.GetComponent<TransformComponent>();

            Vector3 unitPos = transformComponent.Position.ToVector();
            const float speed = 6f;
            float speed2 = speed;// * self.Room().SpeedMultiply;

            if (unitPos != self.Position)
            {
                float distance = (unitPos - self.Position).magnitude;
                self.totalTime = distance / speed2;
                self.t = 0;
                self.Position = transformComponent.Position.ToVector();
                self.Rotation = transformComponent.Rotation.ToQuaternion();
            }
            
            LSInputComponent component = unit.GetComponent<LSInputComponent>();
            if (component != null && component.LSInput.V != TSVector2.zero)
            {
                self.GetComponent<LSAnimatorComponent>().SetFloatValue("Speed", speed2);
            }
            else
            {
                self.GetComponent<LSAnimatorComponent>().SetFloatValue("Speed", 0);
            }
            self.t += Time.deltaTime;
            self.Transform.rotation = Quaternion.Lerp(self.Transform.rotation, self.Rotation, Math.Clamp(self.t / 1f, 0, .99f));
            self.Transform.position = Vector3.Lerp(self.Transform.position, self.Position, self.t / self.totalTime);
        }

        private static LSUnit GetUnit(this LSUnitView self)
        {
            LSUnit unit = self.Unit;
            if (unit != null)
            {
                return unit;
            }

            self.Unit = (self.IScene as Room).LSWorld.GetComponent<LSUnitComponent>().GetChild<LSUnit>(self.Id);
            return self.Unit;
        }
    }
}