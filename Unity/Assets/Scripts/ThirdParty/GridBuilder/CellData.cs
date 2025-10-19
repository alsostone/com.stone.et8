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
        [MemoryPackIgnore] public bool IsFill => isObstacle || contentIds.Count > 0;
    
        [MemoryPackInclude] public List<long> contentIds;
        [MemoryPackInclude] public List<PlacedLayer> contentTypes;
        [MemoryPackInclude] public bool isObstacle;
        [MemoryPackInclude] public IndexV2 index = new IndexV2(-1, -1);
        [MemoryPackInclude] public int distance = int.MaxValue;      // flowField and A*
        [MemoryPackInclude] public FieldV2 direction = new FieldV2(0, 0);       // flowField
        
        [MemoryPackIgnore] public CellData prev = null;             // A*

        public CellData()
        {
            contentIds = new List<long>();
            contentTypes = new List<PlacedLayer>();
        }
    
        [MemoryPackConstructor]
        public CellData(List<long> contentIds, List<PlacedLayer> contentTypes)
        {
            this.contentIds = contentIds;
            this.contentTypes = contentTypes;
        }
    
        public bool CanPut(PlacementData placementData)
        {
            if (isObstacle) {
                return false;
            }
        
            if (contentIds.Count > 0) {
                if (contentIds[^1] == placementData.id)
                    return true;
                if ((placementData.placedLayer & contentTypes[^1]) == 0)
                    return false;
            } else {
                if ((placementData.placedLayer & PlacedLayer.Map) == 0)
                    return false;
            }
            return true;
        }
        
        public bool IsBlock()
        {
            return isObstacle || (contentTypes.Count > 0 && contentTypes[0] == PlacedLayer.Block);
        }
    }

}
