using ST.GridBuilder;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewPlacementComponent : Entity, IAwake<PlacementData>, IDestroy
	{
		public Placement Placement;
	}
}