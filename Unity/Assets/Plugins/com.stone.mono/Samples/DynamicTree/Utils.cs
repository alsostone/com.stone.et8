using TrueSync;
using UnityEngine;

namespace ST.Mono
{
    public static class Utils
    {
        public static Vector3 ToVector3(this TSVector v)
        {
            return new Vector3(v.x.AsFloat(), v.y.AsFloat(), v.z.AsFloat());
        }

        public static TSVector ToTSVector(this Vector3 v)
        {
            return new TSVector(FP.FromFloat(v.x), FP.FromFloat(v.y), FP.FromFloat(v.z));
        }
        
        public static AABB ToAABB(this Bounds bounds)
        {
            return new AABB(bounds.min.ToTSVector(), bounds.max.ToTSVector());
        }
    }
}