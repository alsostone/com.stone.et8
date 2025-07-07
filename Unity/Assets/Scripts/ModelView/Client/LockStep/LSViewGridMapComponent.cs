using ST.GridBuilder;

namespace ET.Client
{
	[ComponentOf]
	public class LSViewGridMapComponent : Entity, IAwake
	{
		public GridMap GridMap { get; set; }
		public GridMapIndicator GridMapIndicator { get; set; }
	}
}