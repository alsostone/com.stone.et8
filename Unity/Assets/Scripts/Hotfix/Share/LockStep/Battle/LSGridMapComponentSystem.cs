using ST.GridBuilder;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSGridMapComponent))]
    [FriendOf(typeof(LSGridMapComponent))]
    public static partial class LSGridMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSGridMapComponent self, GridData gridData)
        {
            self.GridData = gridData;
        }
        
        public static bool TryTake(this LSGridMapComponent self, PlacementData placementData)
        {
            if (self.GridData.CanTake(placementData))
            {
                self.GridData.Take(placementData);
                self.GridData.ResetFlowField();
                return true;
            }
            return false;
        }
        
        public static bool TryPut(this LSGridMapComponent self, int x, int z, PlacementData placementData)
        {
            if (self.GridData.CanPut(x, z, placementData))
            {
                self.GridData.Put(x, z, placementData);
                self.GridData.ResetFlowField();
                return true;
            }
            return false;
        }
        
        public static TSVector GetPutPosition(this LSGridMapComponent self, PlacementData placementData)
        {
            int level = self.GridData.GetPointLevelCount(placementData.x, placementData.z, placementData);
            FieldV2 v2 = self.GridData.GetCellPosition(placementData.x, placementData.z);
            return new TSVector(v2.x, level * self.GridData.cellSize, v2.z);
        }
    }
}