namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class Match2G_MatchSuccessHandler: MessageHandler<Scene, Match2G_MatchSuccess>
    {
        protected override async ETTask Run(Scene root, Match2G_MatchSuccess message)
        {
            root.GetComponent<ClientSenderComponent>().Send(C2Room_JoinRoom.Create(true));
            await ETTask.CompletedTask;
        }
    }
}