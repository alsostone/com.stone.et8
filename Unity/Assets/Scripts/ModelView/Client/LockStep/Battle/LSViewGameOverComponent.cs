
namespace ET.Client
{
	[ComponentOf(typeof(Room))]
	public class LSViewGameOverComponent : Entity, IAwake
	{
		public TeamType WinTeam { get; set; }
		public int EndFrame { get; set; }
		public bool IsWin { get; set; }
	}
}