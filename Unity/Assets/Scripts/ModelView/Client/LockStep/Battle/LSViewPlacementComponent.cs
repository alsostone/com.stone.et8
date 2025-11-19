using ST.GridBuilder;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewPlacementComponent : Entity, IAwake, IDestroy, ILSRollback
	{
		public Placement Placement;
	}
}