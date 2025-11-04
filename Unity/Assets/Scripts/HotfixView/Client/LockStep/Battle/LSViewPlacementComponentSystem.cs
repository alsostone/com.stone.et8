using ST.GridBuilder;

namespace ET.Client
{
	[EntitySystemOf(typeof(LSViewPlacementComponent))]
	[FriendOf(typeof(LSViewPlacementComponent))]
	public static partial class LSViewPlacementComponentSystem
	{
		[EntitySystem]
		private static void Awake(this LSViewPlacementComponent self, PlacementData placementData)
		{
			self.Placement = self.GetParent<LSUnitView>().GameObject.GetComponent<Placement>();
			if (self.Placement)
			{
				LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
				self.Placement.placementData.id = placementData.id;
				self.Placement.placementData.Rotation(placementData.rotation - self.Placement.placementData.rotation);
				gridMapComponent.Put(placementData.x, placementData.z, self.Placement.placementData);
			}
		}
		
        [EntitySystem]
		private static void Destroy(this LSViewPlacementComponent self)
		{
			if (self.Placement)
			{
				LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
				gridMapComponent.Take(self.Placement.placementData);
			}
		}
		
		public static void Placed(this LSViewPlacementComponent self, int x, int z)
		{
			if (self.Placement)
			{
				LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
				gridMapComponent.Take(self.Placement.placementData);
				gridMapComponent.Put(x, z, self.Placement.placementData);
			}
		}
	}
}