using System;

namespace TrueSync
{
    /// <summary>
    /// Represents an axis aligned bounding box.
    /// </summary>
    [Serializable]
    public struct TSBounds
    {
        private TSVector m_Center;
        private TSVector m_Extents;
        
        /// <summary>
        ///   <para>Creates a new Bounds.</para>
        /// </summary>
        /// <param name="center">The location of the origin of the Bounds.</param>
        /// <param name="size">The dimensions of the Bounds.</param>
        public TSBounds(TSVector center, TSVector size)
        {
            this.m_Center = center;
            this.m_Extents = size * FP.Half;
        }
        
        /// <summary>
        ///   <para>The center of the bounding box.</para>
        /// </summary>
        public TSVector center
        {
            get => this.m_Center;
            set => this.m_Center = value;
        }

        /// <summary>
        ///   <para>The total size of the box. This is always twice as large as the extents.</para>
        /// </summary>
        public TSVector size
        {
            get => this.m_Extents * 2;
            set => this.m_Extents = value * FP.Half;
        }
        
        
        /// <summary>
        ///   <para>The extents of the Bounding Box. This is always half of the size of the Bounds.</para>
        /// </summary>
        public TSVector extents
        {
            get => this.m_Extents;
            set => this.m_Extents = value;
        }

        /// <summary>
        ///   <para>The minimal point of the box. This is always equal to center-extents.</para>
        /// </summary>
        public TSVector min
        {
            get => this.center - this.extents;
            set => this.SetMinMax(value, this.max);
        }

        /// <summary>
        ///   <para>The maximal point of the box. This is always equal to center+extents.</para>
        /// </summary>
        public TSVector max
        {
            get => this.center + this.extents;
            set => this.SetMinMax(this.min, value);
        }
        
        /// <summary>
        ///   <para>Sets the bounds to the min and max value of the box.</para>
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetMinMax(TSVector min, TSVector max)
        {
            this.extents = (max - min) * FP.Half;
            this.center = min + this.extents;
        }
        
        /// <summary>
        ///   <para>Grows the Bounds to include the point.</para>
        /// </summary>
        /// <param name="point"></param>
        public void Encapsulate(TSVector point) => this.SetMinMax(TSVector.Min(this.min, point), TSVector.Max(this.max, point));
        
        /// <summary>
        ///   <para>Grow the bounds to encapsulate the bounds.</para>
        /// </summary>
        /// <param name="bounds"></param>
        public void Encapsulate(TSBounds bounds)
        {
            this.Encapsulate(bounds.center - bounds.extents);
            this.Encapsulate(bounds.center + bounds.extents);
        }
        
        /// <summary>
        ///   <para>Expand the bounds by increasing its size by amount along each side.</para>
        /// </summary>
        /// <param name="amount"></param>
        public void Expand(FP amount)
        {
            amount *= FP.Half;
            this.extents += new TSVector(amount, amount, amount);
        }
        
        /// <summary>
        ///   <para>Expand the bounds by increasing its size by amount along each side.</para>
        /// </summary>
        /// <param name="amount"></param>
        public void Expand(TSVector amount) => this.extents += amount * FP.Half;
        
        /// <summary>
        ///   <para>Does another bounding box intersect with this bounding box?</para>
        /// </summary>
        /// <param name="bounds"></param>
        public bool Intersects(TSBounds bounds) => this.min.x <= bounds.max.x 
                                                   && this.max.x >= bounds.min.x
                                                   && this.min.y <= bounds.max.y 
                                                   && this.max.y >= bounds.min.y 
                                                   && this.min.z <= bounds.max.z 
                                                   && this.max.z >= bounds.min.z;

        public bool Contains(TSVector point)
        {
            return this.min.x <= point.x 
                   && this.max.x >= point.x
                   && this.min.y <= point.y 
                   && this.max.y >= point.y 
                   && this.min.z <= point.z 
                   && this.max.z >= point.z;
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for the bounds.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString() => string.Format("Center: {0}, Extents: {1}", (object) this.m_Center, (object) this.m_Extents);

        public override int GetHashCode()
        {
            TSVector vector3 = this.center;
            int hashCode = vector3.GetHashCode();
            vector3 = this.extents;
            int num = vector3.GetHashCode() << 2;
            return hashCode ^ num;
        }

        public override bool Equals(object other) => other is TSBounds other1 && this.Equals(other1);
        
        public bool Equals(TSBounds other) => this.center.Equals(other.center) && this.extents.Equals(other.extents);
        
        public static bool operator ==(TSBounds lhs, TSBounds rhs) => lhs.center == rhs.center && lhs.extents == rhs.extents;

        public static bool operator !=(TSBounds lhs, TSBounds rhs) => !(lhs == rhs);
    }
}