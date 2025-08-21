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
        }

        [EntitySystem]
        private static void Deserialize(this PlacementComponent self)
        {
            if (self.PlacementData.id > 0)
            {
                LSGridMapComponent component = self.LSWorld().GetComponent<LSGridMapComponent>();
                component.Put(self.PlacementData.x, self.PlacementData.z, self.PlacementData);
            }
        }

        public static PlacementData GetPlacementData(this PlacementComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(86, self.LSParent().Id);
            return self.PlacementData;
        }
    }
}