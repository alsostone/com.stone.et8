
namespace ET.Client
{
	[EntitySystemOf(typeof(LSLookComponent))]
	[FriendOf(typeof(LSLookComponent))]
	public static partial class LSLookComponentSystem
	{
		[EntitySystem]
		private static void Awake(this LSLookComponent self, long ownerPlayerId, long lookPlayerId)
		{
			self.OwnerPlayerId = ownerPlayerId;
			self.LookPlayerId = lookPlayerId == 0 ? self.Room().PlayerIds[0] : lookPlayerId;
		}
		
		public static void SetLookSeatIndex(this LSLookComponent self, int lookSeatIndex)
		{
			Room room = self.Room();
			self.LookPlayerId = room.PlayerIds[lookSeatIndex % room.PlayerIds.Count];
		}

		public static int GetOwnerSeatIndex(this LSLookComponent self)
		{
			Room room = self.Room();
			return room.PlayerIds.IndexOf(self.OwnerPlayerId);
		}

		public static byte GetLookSeatIndex(this LSLookComponent self)
		{
			Room room = self.Room();
			return (byte)room.PlayerIds.IndexOf(self.LookPlayerId);
		}

	}
}
