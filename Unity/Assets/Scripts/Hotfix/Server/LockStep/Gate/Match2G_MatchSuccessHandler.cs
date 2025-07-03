using System;


namespace ET.Server
{
	[MessageHandler(SceneType.Gate)]
	public class Match2G_MatchSuccessHandler : MessageHandler<Player, Match2G_MatchSuccess>
	{
		protected override async ETTask Run(Player player, Match2G_MatchSuccess message)
		{
			PlayerRoomComponent playerRoomComponent = player.AddComponent<PlayerRoomComponent>();
			playerRoomComponent.RoomActorId = message.ActorId;
			playerRoomComponent.RoomSeatIndex = message.SeatIndex;
			
			player.GetComponent<PlayerSessionComponent>().Session.Send(message);
			await ETTask.CompletedTask;
		}
	}
}