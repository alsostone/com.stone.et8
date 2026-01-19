using System.Runtime.CompilerServices;
using TrueSync;

namespace ST.Collision
{
    public struct AABB
    {
        public TSVector LowerBound;
        public TSVector UpperBound;

        public AABB(TSVector lowerBound, TSVector upperBound)
        {
            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }
        
        /// <summary>
        ///     Get the center of the AABB.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSVector GetCenter()
        {
            return FP.Half * (LowerBound + UpperBound);
        }

        /// <summary>
        ///     Get the extents of the AABB (half-widths).
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TSVector GetExtents()
        {
            return FP.Half * (UpperBound - LowerBound);
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
        

    }
}
