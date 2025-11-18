using ST.GridBuilder;

namespace ET.Client
{
	[ComponentOf(typeof(Room))]
	public class LSViewGridMapComponent : Entity, IAwake, ILSRollback
	{
		public GridMap GridMap { get; set; }
		public GridMapIndicator GridMapIndicator { get; set; }
	}
}