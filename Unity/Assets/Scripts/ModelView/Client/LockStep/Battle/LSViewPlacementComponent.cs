using ST.GridBuilder;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewPlacementComponent : Entity, IAwake, IDestroy
	{
		public Placement Placement;
	}
}