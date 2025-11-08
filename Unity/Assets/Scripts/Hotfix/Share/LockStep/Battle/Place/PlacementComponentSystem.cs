using ST.GridBuilder;

namespace ET
{
    [EntitySystemOf(typeof(PlacementComponent))]
    [FriendOf(typeof(PlacementComponent))]
    public static partial class PlacementComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PlacementComponent self, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(87, self.LSParent().Id);
            self.PlacementData = placementData;
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            gridMapComponent.Put(placementData.x, placementData.z, placementData);
        }
        
        [EntitySystem]
        private static void Destroy(this PlacementComponent self)
        {
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            gridMapComponent.Take(self.PlacementData);
         }
        
        [EntitySystem]
        private static void Deserialize(this PlacementComponent self)
        {
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            gridMapComponent.Put(self.PlacementData.x, self.PlacementData.z, self.PlacementData);
        }

        public static PlacementData GetPlacementData(this PlacementComponent self)
        {
            return self.PlacementData;
        }
    }
}