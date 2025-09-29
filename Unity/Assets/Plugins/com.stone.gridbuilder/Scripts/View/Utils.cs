using TrueSync;
using UnityEngine;

namespace ST.GridBuilder
{
    public static class Utils
    {
        public static Vector3 ToVector3(this FieldV2 v2)
        {
            return new Vector3(v2.x.AsFloat(), 0, v2.z.AsFloat());
        }
        
        public static FieldV2 ToFieldV2(this Vector3 v3)
        {
            return new FieldV2((FP)v3.x, (FP)v3.z);
        }

        public static TSVector ToTSVector(this Vector3 v3)
        {
            return new TSVector((FP)v3.x, (FP)v3.y, (FP)v3.z);
        }
        
    }
}