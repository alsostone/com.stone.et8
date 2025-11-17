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
            self.AddRVO2Object();
        }

        [EntitySystem]
        private static void Destroy(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(98, self.LSParent().Id);
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            gridMapComponent.Take(self.PlacementData);
            self.RemoveRVO2Object();
        }

        [EntitySystem]
        private static void Deserialize(this PlacementComponent self)
        {
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            gridMapComponent.Put(self.PlacementData.x, self.PlacementData.z, self.PlacementData);
            self.AddRVO2Object();
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
                self.RemoveRVO2Object();
                self.AddRVO2Object();
                EventSystem.Instance.Publish(self.LSWorld(), new LSUnitPlaced() { Id = lsOwner.Id, X = index.x, Z = index.z });
                lsOwner.GetComponent<TransformComponent>().SetPosition(lsGridMapComponent.GetPutPosition(self.PlacementData));
            }
        }

        private static void AddRVO2Object(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(157, self.LSParent().Id);
            switch (self.PlacementData.placementType)
            {
                case PlacedLayer.Block:
                {
                    LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                    List<TSVector2> vertices = ObjectPool.Instance.Fetch<List<TSVector2>>();
                    self.LSWorld().GetComponent<LSGridMapComponent>().GenPlacementBoundary(self.PlacementData, vertices);
                    rvo2Component.AddObstacle(self.LSUnit(self.PlacementData.id), vertices);
                    vertices.Clear();
                    ObjectPool.Instance.Recycle(vertices);
                    break;
                }
                case PlacedLayer.Building:
                {
                    LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                    rvo2Component.AddStaticAgent(self.LSUnit(self.PlacementData.id));
                    break;
                }
            }
        }
        
        private static void RemoveRVO2Object(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(156, self.LSParent().Id);
            switch (self.PlacementData.placementType)
            {
                case PlacedLayer.Block: {
                    LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                    rvo2Component.RemoveObstacle(self.LSUnit(self.PlacementData.id));
                    break;
                }
                case PlacedLayer.Building: {
                    LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                    rvo2Component.RemoveAgent(self.LSUnit(self.PlacementData.id));
                    break;
                }
            }
        }
        
    }
}