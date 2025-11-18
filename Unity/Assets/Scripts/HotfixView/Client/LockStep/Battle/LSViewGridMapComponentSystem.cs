using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
    [LSEntitySystemOf(typeof(LSViewGridMapComponent))]
    [EntitySystemOf(typeof(LSViewGridMapComponent))]
    [FriendOf(typeof(LSViewGridMapComponent))]
    public static partial class LSViewGridMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewGridMapComponent self)
        {
            self.GridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
            LSGridMapComponent lsGridMapComponent = self.Room().LSWorld.GetComponent<LSGridMapComponent>();
            self.RebindGridDataDraw(lsGridMapComponent.GetGridData(), lsGridMapComponent.GetDefaultFlowField());
            
            self.GridMapIndicator = UnityEngine.Object.FindObjectOfType<GridMapIndicator>();
            if (self.GridMapIndicator)
                self.GridMapIndicator.SetGridMap(self.GridMap);
        }
        
        [LSEntitySystem]
        private static void LSRollback(this LSViewGridMapComponent self)
        {
            LSGridMapComponent lsGridMapComponent = self.Room().LSWorld.GetComponent<LSGridMapComponent>();
            self.RebindGridDataDraw(lsGridMapComponent.GetGridData(), lsGridMapComponent.GetDefaultFlowField());
        }
        
        public static void RebindGridDataDraw(this LSViewGridMapComponent self, GridData gridData, FlowFieldNode[] flowField)
        {
            if (self.GridMap == null)
                return;
            
            // 表现层的GridData没有流场数据 因为表现层没必要维护它
            // 所以这里使用逻辑层的GridData来做绘制数据源
            self.GridMap.gridDataDraw = gridData;
            self.GridMap.flowFieldDraw = flowField;
        }
        
        public static Quaternion GetGridRotation(this LSViewGridMapComponent self)
        {
            return self.GridMap.GetGridRotation();
        }
        
        public static IndexV2 ConvertToIndex(this LSViewGridMapComponent self, Vector3 position)
        {
            return self.GridMap.ConvertToIndex(position);
        }

        public static Vector3 GetLevelPosition(this LSViewGridMapComponent self, int x, int z, int level, float height = 0)
        {
            return self.GridMap.GetLevelPosition(x, z, level, height);
        }
        
        public static int GetShapeLevelCount(this LSViewGridMapComponent self, int x, int z, PlacementData placementData)
        {
            return self.GridMap.gridData.GetShapeLevelCount(x, z, placementData);
        }
        
        public static bool CanTake(this LSViewGridMapComponent self, PlacementData placementData)
        {
            return self.GridMap.gridData.CanTake(placementData);
        }
        
        public static void Take(this LSViewGridMapComponent self, PlacementData placementData)
        {
            self.GridMap.gridData.Take(placementData);
        }
        
        public static void Put(this LSViewGridMapComponent self, int x, int z, PlacementData placementData)
        {
            self.GridMap.gridData.Put(x, z, placementData);
        }

        public static void GenerateIndicator(this LSViewGridMapComponent self, int x, int z, int targetLevel, PlacementData placementData)
        {
            if (self.GridMapIndicator)
                self.GridMapIndicator.GenerateIndicator(x, z, targetLevel, placementData);
        }
        
        public static void ClearIndicator(this LSViewGridMapComponent self)
        {
            if (self.GridMapIndicator)
                self.GridMapIndicator.ClearIndicator();
        }
    }
}