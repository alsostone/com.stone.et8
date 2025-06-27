using System;
using System.Collections.Generic;
using MemoryPack;

namespace ST.GridBuilder
{
    [Serializable]
    public struct FieldV2
    {
        public float x;
        public float z;

        public FieldV2(float x, float z)
        {
            this.x = x;
            this.z = z;
        }
        public FieldV2 Lerp(FieldV2 b, float t)
        {
            return new FieldV2(
                x + (b.x - x) * t,
                z + (b.z - z) * t
            );
        }
        public FieldV2 Normalize()
        {
            float length = (float)Math.Sqrt(x * x + z * z);
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
        [MemoryPackInclude] public int distance = int.MaxValue;
        [MemoryPackInclude] public FieldV2 direction = new FieldV2(0, 0);

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
    }

}
