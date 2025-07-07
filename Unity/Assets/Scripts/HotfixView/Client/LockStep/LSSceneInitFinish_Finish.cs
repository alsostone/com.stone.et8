namespace ET.Client
{
    [Event(SceneType.LockStep)]
    public class LSSceneInitFinish_Finish: AEvent<Scene, LSSceneInitFinish>
    {
        protected override async ETTask Run(Scene clientScene, LSSceneInitFinish args)
        {
            Room room = clientScene.GetComponent<Room>();
            
            room.AddComponent<LSUnitViewComponent>();
            room.AddComponent<LSCameraComponent>();

            if (!room.IsReplay)
            {
                long myId = room.Root().GetComponent<PlayerComponent>().MyId;
                room.AddComponent<LSCommandsComponent, byte>(room.GetSeatIndex(myId));
                room.AddComponent<LSOperaComponent>();
                room.AddComponent<LSOperaDragComponent>();
                room.AddComponent<LSClientUpdater>();
            }
            else
            {
                room.AddComponent<LSReplayUpdater>();
            }
            await YIUIMgrComponent.Inst.ClosePanelAsync<LSLobbyPanelComponent>();
            await ETTask.CompletedTask;
        }
    }
}