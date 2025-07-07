using ST.GridBuilder;

namespace ET.Client
{
	[EntitySystemOf(typeof(LSViewPlacementComponent))]
	[FriendOf(typeof(LSViewPlacementComponent))]
	public static partial class LSViewPlacementComponentSystem
	{
		[EntitySystem]
		private static void Awake(this LSViewPlacementComponent self)
		{
			self.Placement = self.GetParent<LSUnitView>().GameObject.GetComponent<Placement>();
		}
		
        [EntitySystem]
		private static void Destroy(this LSViewPlacementComponent self)
		{

		}
	}
}