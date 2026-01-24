using System.Runtime.CompilerServices;
using MemoryPack;
using TrueSync;

namespace ST.Mono
{
    [MemoryPackable]
    public partial struct AABB
    {
        public TSVector LowerBound;
        public TSVector UpperBound;
        
        public TSVector Size => UpperBound - LowerBound;
        public TSVector Center => FP.Half * (LowerBound + UpperBound);
        public TSVector Extents => FP.Half * (UpperBound - LowerBound);

        public AABB(TSVector lowerBound, TSVector upperBound)
        {
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }
        
        public readonly FP GetSurfaceArea()
        {
            FP x_size = UpperBound.x - LowerBound.x;
            FP y_size = UpperBound.y - LowerBound.y;
            FP z_size = UpperBound.z - LowerBound.z;

            return FP.Two * ((x_size * y_size) + (x_size * z_size) + (y_size * z_size));
        }
        
        public static void Combine(in AABB left, in AABB right, out AABB aabb)
        {
            aabb = new AABB(
                TSVector.Min(left.LowerBound, right.LowerBound),
                TSVector.Max(left.UpperBound, right.UpperBound));
        }
        
        /// <summary>
        ///     Combine an AABB into this one.
        /// </summary>
        /// <param name="aabb"></param>
        public void Combine(in AABB aabb)
        {
            LowerBound = TSVector.Min(LowerBound, aabb.LowerBound);
            UpperBound = TSVector.Max(UpperBound, aabb.UpperBound);
        }
        
        /// <summary>
        ///     Combine two AABBs into this one.
        /// </summary>
        /// <param name="aabb1"></param>
        /// <param name="aabb2"></param>
        public void Combine(in AABB aabb1, in AABB aabb2)
        {
            LowerBound = TSVector.Min(aabb1.LowerBound, aabb2.LowerBound);
            UpperBound = TSVector.Max(aabb1.UpperBound, aabb2.UpperBound);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(AABB aabb)
        {
            return LowerBound.x <= aabb.LowerBound.x && UpperBound.x >= aabb.UpperBound.x
                    && LowerBound.y <= aabb.LowerBound.y && UpperBound.y >= aabb.UpperBound.y
                    && LowerBound.z <= aabb.LowerBound.z && UpperBound.z >= aabb.UpperBound.z;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(AABB aabb)
        {
            return LowerBound.x <= aabb.UpperBound.x && UpperBound.x >= aabb.LowerBound.x
                    && LowerBound.y <= aabb.UpperBound.y && UpperBound.y >= aabb.LowerBound.y 
                    && LowerBound.z <= aabb.UpperBound.z && UpperBound.z >= aabb.LowerBound.z;
        }

        // tmin为负表示起点在盒子内
        public bool RayCast(TSVector p1, TSVector p2, out FP tmin)
        {
            tmin = FP.MinValue;
            FP tmax = FP.MaxValue;
            TSVector dir = p2 - p1;
            
            if (FP.Abs(dir.x) < FP.Epsilon) {
                if (p1.x < LowerBound.x || p1.x > UpperBound.x) {
                    return false;
                }
            } else {
                FP ood = FP.One / dir.x;
                FP t1 = (LowerBound.x - p1.x) * ood;
                FP t2 = (UpperBound.x - p1.x) * ood;
                
                if (t1 > t2) (t1, t2) = (t2, t1);
                if (t1 > tmin) tmin = t1;
                if (t2 < tmax) tmax = t2;

                if (tmin > tmax)
                    return false;
            }
            
            if (FP.Abs(dir.y) < FP.Epsilon) {
                if (p1.y < LowerBound.y || p1.y > UpperBound.y) {
                    return false;
                }
            } else {
                FP ood = FP.One / dir.y;
                FP t1 = (LowerBound.y - p1.y) * ood;
                FP t2 = (UpperBound.y - p1.y) * ood;
                
                if (t1 > t2) (t1, t2) = (t2, t1);
                if (t1 > tmin) tmin = t1;
                if (t2 < tmax) tmax = t2;

                if (tmin > tmax)
                    return false;
            }
            
            if (FP.Abs(dir.z) < FP.Epsilon) {
                if (p1.z < LowerBound.z || p1.z > UpperBound.z) {
                    return false;
                }
            } else {
                FP ood = FP.One / dir.z;
                FP t1 = (LowerBound.z - p1.z) * ood;
                FP t2 = (UpperBound.z - p1.z) * ood;
                
                if (t1 > t2) (t1, t2) = (t2, t1);
                if (t1 > tmin) tmin = t1;
                if (t2 < tmax) tmax = t2;

                if (tmin > tmax)
                    return false;
            }

            return true;
        }

    }
}
