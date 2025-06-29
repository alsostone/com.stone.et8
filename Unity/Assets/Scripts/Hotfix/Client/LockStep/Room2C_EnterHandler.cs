namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class Room2C_EnterHandler: MessageHandler<Scene, Room2C_Enter>
    {
        protected override async ETTask Run(Scene root, Room2C_Enter message)
        {
            await LSSceneChangeHelper.SceneChangeTo(root, message.MatchInfo);
        }
    }
}