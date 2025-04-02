using System;
using ET.Client;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSInputComponent))]
    [LSEntitySystemOf(typeof(LSInputComponent))]
    public static partial class LSInputComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSInputComponent self)
        {

        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSInputComponent self)
        {
            LSUnit unit = self.Owner;
            PropComponent numericComponent = unit.GetComponent<PropComponent>();
            FP speed = numericComponent.GetByKey(NumericType.Speed) / LSConstValue.PropPrecision;
            TSVector2 v2 = self.LSInput.V * speed * LSConstValue.UpdateInterval / 1000;
            if (v2.LengthSquared() > FP.EN4)
            {
                TSVector oldPos = unit.Position;
                unit.Position += new TSVector(v2.x, 0, v2.y);
                unit.Forward = unit.Position - oldPos;
            }
            
        }
    }
}