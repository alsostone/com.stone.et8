using System.Collections.Generic;
using ST.GridBuilder;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(PlacementComponent))]
    [FriendOf(typeof(PlacementComponent))]
    public static partial class PlacementComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PlacementComponent self, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(99, self.LSParent().Id);
            self.PlacementData = placementData;
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            gridMapComponent.Put(placementData.x, placementData.z, placementData);
            switch (self.PlacementData.placementType) {
                case PlacedLayer.Block: self.AddRVO2Obstacle(); break;
                case PlacedLayer.Building: self.AddRVO2Agent(); break;
            }
        }

        [EntitySystem]
        private static void Destroy(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(98, self.LSParent().Id);
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            gridMapComponent.Take(self.PlacementData);
            switch (self.PlacementData.placementType) {
                case PlacedLayer.Block: self.RemoveRVO2Obstacle(); break;
                case PlacedLayer.Building: self.RemoveRVO2Agent(); break;
            }
        }

        [EntitySystem]
        private static void Deserialize(this PlacementComponent self)
        {
            switch (self.PlacementData.placementType) {
                case PlacedLayer.Block: self.AddRVO2Obstacle(); break;
                case PlacedLayer.Building: self.AddRVO2Agent(); break;
            }
        }

        public static PlacementData GetPlacementData(this PlacementComponent self)
        {
            return self.PlacementData;
        }

        public static void PutToPosition(this PlacementComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(158, self.LSParent().Id, position.x.V, position.y.V, position.z.V);
            LSUnit lsOwner = self.LSOwner();
            LSGridMapComponent lsGridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            IndexV2 index = lsGridMapComponent.ConvertToIndex(position);
            if (lsGridMapComponent.CanPut(index.x, index.z, self.PlacementData) && lsGridMapComponent.CanTake(self.PlacementData))
            {
                lsGridMapComponent.Take(self.PlacementData);
                lsGridMapComponent.Put(index.x, index.z, self.PlacementData);
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitPlaced() { Id = lsOwner.Id, X = index.x, Z = index.z });

                // 设置位置到TransformComponent
                TSVector putPosition = lsGridMapComponent.GetPutPosition(self.PlacementData);
                lsOwner.GetComponent<TransformComponent>().SetPosition(putPosition, true);
                
                // 更新RVO实体的位置 障碍物特殊，需要重新创建
                switch (self.PlacementData.placementType) {
                    case PlacedLayer.Block:
                        self.RemoveRVO2Obstacle();
                        self.AddRVO2Obstacle();
                        break;
                    case PlacedLayer.Building:
                        self.SetRVO2AgentPosition(putPosition);
                        break;
                }
            }
        }

        private static void AddRVO2Agent(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(163, self.LSParent().Id);
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.AddStaticAgent(self.LSUnit(self.PlacementData.id));
        }
        
        private static void RemoveRVO2Agent(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(162, self.LSParent().Id);
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.RemoveAgent(self.LSUnit(self.PlacementData.id));
        }
        
        private static void SetRVO2AgentPosition(this PlacementComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(161, self.LSParent().Id, position.x.V, position.y.V, position.z.V);
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.SetAgentPosition(self.LSUnit(self.PlacementData.id), new TSVector2(position.x, position.z));
        }
        
        private static void AddRVO2Obstacle(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(160, self.LSParent().Id);
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            List<TSVector2> vertices = ObjectPool.Instance.Fetch<List<TSVector2>>();
            self.LSWorld().GetComponent<LSGridMapComponent>().GenPlacementBoundary(self.PlacementData, vertices);
            rvo2Component.AddObstacle(self.LSUnit(self.PlacementData.id), vertices);
            vertices.Clear();
            ObjectPool.Instance.Recycle(vertices);
        }

        private static void RemoveRVO2Obstacle(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(159, self.LSParent().Id);
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            rvo2Component.RemoveObstacle(self.LSUnit(self.PlacementData.id));
        }

    }
}