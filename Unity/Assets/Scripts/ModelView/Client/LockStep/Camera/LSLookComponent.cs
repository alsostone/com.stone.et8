
namespace ET.Client
{
	[ComponentOf(typeof(Room))]
	public class LSLookComponent : Entity, IAwake<long, long>
	{
		// 当前玩家ID (登陆后是自己，未登陆为默认值0)
		public long OwnerPlayerId { get; set; }
		
		// 当前看向的玩家ID（一般是自己，回放时看是谁的录像）
		public long LookPlayerId { get; set; }
		
	}
}
