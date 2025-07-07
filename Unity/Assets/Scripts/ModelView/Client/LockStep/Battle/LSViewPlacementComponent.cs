using ST.GridBuilder;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewPlacementComponent : Entity, IAwake<long, int, int>, IDestroy
	{
		public Placement Placement;
	}
}