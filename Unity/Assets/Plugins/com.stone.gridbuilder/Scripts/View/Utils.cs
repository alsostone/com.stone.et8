using UnityEngine;

namespace ST.GridBuilder
{
    public static class Utils
    {
        public static Vector3 ToVector3(this FieldV2 v2)
        {
            return new Vector3(v2.x, 0, v2.z);
        }
        
        public static FieldV2 ToFieldV2(this Vector3 v3)
        {
            return new FieldV2(v3.x, v3.z);
        }
    }
}