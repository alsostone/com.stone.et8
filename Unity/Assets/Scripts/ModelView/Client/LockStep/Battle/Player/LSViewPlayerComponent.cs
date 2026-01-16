
namespace ET.Client
{
	[ComponentOf]
	public class LSViewPlayerComponent : Entity, IAwake<long, long>
	{
		public long BindCampId { get; set; }
		public long BindHeroId { get; set; }
	}
}