namespace ET.Server
{
	[MessageSessionHandler(SceneType.Gate)]
	public class C2G_MatchHandler : MessageSessionHandler<C2G_Match, G2C_Match>
	{
		protected override async ETTask Run(Session session, C2G_Match request, G2C_Match response)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().Player;

			TbStartSceneRow tbStartSceneRow = TbStartScene.Instance.Match;

			G2Match_Match g2MatchMatch = G2Match_Match.Create();
			g2MatchMatch.Id = player.Id;
			g2MatchMatch.StageId = request.StageId;
			await session.Root().GetComponent<MessageSender>().Call(tbStartSceneRow.ActorId, g2MatchMatch);
		}
	}
}