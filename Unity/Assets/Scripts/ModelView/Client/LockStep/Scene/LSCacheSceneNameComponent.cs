
namespace ET.Client
{
	[ComponentOf(typeof(Room))]
	public class LSCacheSceneNameComponent : Entity, IAwake<string>
	{
		public string Name { get; set; }
	}
}
