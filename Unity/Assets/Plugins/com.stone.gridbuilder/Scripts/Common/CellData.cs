using System;
using System.Collections.Generic;

[Serializable]
public class CellData
{
    public bool isFill => isObstacle || contentIds.Count > 0;
    
    public List<long> contentIds;
    public List<PlacedLayer> contentTypes;
    public bool isObstacle;

    public CellData()
    {
        contentIds = new List<long>();
    }
    
    public CellData(List<long> buildingIds)
    {
        this.contentIds = buildingIds;
    }
    
    public bool CanPut(PlacementData placementData)
    {
        if (isObstacle) {
            return false;
        }
        
        if (contentIds.Count > 0) {
            if (contentIds[^1] == placementData.Id)
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
