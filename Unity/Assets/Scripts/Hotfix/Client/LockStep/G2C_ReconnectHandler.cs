namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class G2C_ReconnectHandler: MessageHandler<Scene, G2C_Reconnect>
    {
        protected override async ETTask Run(Scene root, G2C_Reconnect message)
        {
            long ownerPlayerId = root.GetComponent<PlayerComponent>().MyId;
            await LSSceneChangeHelper.SceneChangeToReconnect(root, message, ownerPlayerId, ownerPlayerId);
            await ETTask.CompletedTask;
        }
    }
}