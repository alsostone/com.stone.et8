using ST.GridBuilder;

namespace ET.Client
{
	[LSEntitySystemOf(typeof(LSViewPlacementComponent))]
	[EntitySystemOf(typeof(LSViewPlacementComponent))]
	[FriendOf(typeof(LSViewPlacementComponent))]
	public static partial class LSViewPlacementComponentSystem
	{
		// 实际上表现层的放置数据可以直接从逻辑层获取，不需要Put、Take操作，也不需要Rollback
		// 为了后续可能的扩展性（比如分线程、防止直接修改逻辑层数据等），先保留这些操作
		
		[EntitySystem]
		private static void Awake(this LSViewPlacementComponent self)
		{
			self.Placement = self.GetParent<LSUnitView>().GameObject.GetComponent<Placement>();
			if (self.Placement)
			{
				LSUnit lsUnit = self.LSViewOwner().GetUnit();
				PlacementData placementData = lsUnit.GetComponent<PlacementComponent>().GetPlacementData();
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
		
		[LSEntitySystem]
		private static void LSRollback(this LSViewPlacementComponent self)
		{
			if (self.Placement)
			{
				LSUnit lsUnit = self.LSViewOwner().GetUnit();
				PlacementData placementData = lsUnit.GetComponent<PlacementComponent>().GetPlacementData();
				PlacementData placementDataView = self.Placement.placementData;
				if (placementData.x != placementDataView.x || placementData.z != placementDataView.z)
				{
					LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
					gridMapComponent.Take(placementDataView);
					gridMapComponent.Put(placementData.x, placementData.z, self.Placement.placementData);
				}
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