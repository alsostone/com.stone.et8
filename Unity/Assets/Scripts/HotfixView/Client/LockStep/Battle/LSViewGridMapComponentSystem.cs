using ST.GridBuilder;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewGridMapComponent))]
    [FriendOf(typeof(LSViewGridMapComponent))]
    public static partial class LSViewGridMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewGridMapComponent self)
        {
            self.GridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
            self.RebindGridDataDraw();
            
            self.GridMapIndicator = UnityEngine.Object.FindObjectOfType<GridMapIndicator>();
            if (self.GridMapIndicator)
                self.GridMapIndicator.SetGridMap(self.GridMap);
        }
        
        public static void RebindGridDataDraw(this LSViewGridMapComponent self)
        {
            if (self.GridMap == null)
                return;
            
            // 表现层的GridData没有流场数据 因为表现层没必要维护它
            // 所以这里使用逻辑层的GridData来做绘制数据源
            LSWorld lsWorld = self.Room().LSWorld;
            LSGridMapComponent lsGridMapComponent = lsWorld.GetComponent<LSGridMapComponent>();
            self.GridMap.gridDataDraw = lsGridMapComponent.GridData;
        }
        
    }
}