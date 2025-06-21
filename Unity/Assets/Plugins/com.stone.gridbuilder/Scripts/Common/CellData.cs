using System;
using System.Collections.Generic;

[Serializable]
public class CellData
{
    public bool isFill => isObstacle || contentIds.Count > 0;
    
    public List<long> contentIds;
    public bool isObstacle;
    
    public CellData()
    {
        contentIds = new List<long>();
    }
    
    public CellData(List<long> buildingIds)
    {
        this.contentIds = buildingIds;
    }
}
