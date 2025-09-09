namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class Room2C_EnterHandler: MessageHandler<Scene, Room2C_Enter>
    {
        protected override async ETTask Run(Scene root, Room2C_Enter message)
        {
            long ownerPlayerId = root.GetComponent<PlayerComponent>().MyId;
            await LSSceneChangeHelper.SceneChangeTo(root, LockStepMode.Server, message.MatchInfo, ownerPlayerId, ownerPlayerId);
        }
    }
}