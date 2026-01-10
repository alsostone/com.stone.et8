using System;
using System.Collections.Generic;
using MemoryPack;
using TrueSync;

namespace ST.GridBuilder
{
    [MemoryPackable]
    [Serializable]
    public partial struct FieldV2
    {
        [MemoryPackInclude] public FP x;
        [MemoryPackInclude] public FP z;

        public FieldV2(FP x, FP z)
        {
            this.x = x;
            this.z = z;
        }
        public FieldV2 Lerp(FieldV2 b, FP t)
        {
            return new FieldV2(
                x + (b.x - x) * t,
                z + (b.z - z) * t
            );
        }
        public FieldV2 Normalize()
        {
            FP length = TSMath.Sqrt(x * x + z * z);
            if (length == 0) return new FieldV2(0, 0);
            return new FieldV2(x / length, z / length);
        }
    }
    
    [MemoryPackable]
    [Serializable]
    public partial class CellData
    {
        [MemoryPackInclude] public bool isObstacle;
        [MemoryPackInclude] public IndexV2 index = new IndexV2(-1, -1);
        
        [MemoryPackIgnore, NonSerialized] public int cost = int.MaxValue;      // A*
        [MemoryPackIgnore, NonSerialized] public CellData prev = null;         // A*
    }

}
