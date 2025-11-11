using System;

namespace ET.Client
{
    [Event(SceneType.LockStep)]
    public class LSSceneInitFinish_Finish: AEvent<Scene, LSSceneInitFinish>
    {
        protected override async ETTask Run(Scene clientScene, LSSceneInitFinish args)
        {
            Room room = clientScene.GetComponent<Room>();
            
            room.AddComponent<LSSettingsComponent>();
            room.AddComponent<LSCameraComponent>();
            room.AddComponent<LSViewGameOverComponent>();
            room.AddComponent<LSViewGridMapComponent>();
            room.AddComponent<LSUnitViewComponent>();

            switch (room.LockStepMode)
            {
                case LockStepMode.ObserverFile:
                case LockStepMode.ObserverReport:
                {
                    room.AddComponent<LSReplayUpdater>();
                    break;
                }
                case LockStepMode.Local:
                {
                    room.AddComponent<LSCommandsComponent, byte>((byte)room.GetOwnerSeatIndex());
                    room.AddComponent<LSOperaComponent>();
                    room.AddComponent<LSOperaDragComponent>();
                    room.AddComponent<LSClientLocalUpdater>();
                    break;
                }
                case LockStepMode.Server:
                {
                    room.AddComponent<LSCommandsComponent, byte>((byte)room.GetOwnerSeatIndex());
                    room.AddComponent<LSOperaComponent>();
                    room.AddComponent<LSOperaDragComponent>();
                    room.AddComponent<LSClientServerUpdater>();
                    break;
                }
                case LockStepMode.Observer:
                {
                    room.AddComponent<LSClientServerUpdater>();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // 创建房间UI
            YIUIRootComponent yiuiRootComponent = room.GetComponent<YIUIRootComponent>();
            var viewIndex = room.LockStepMode < LockStepMode.Local ? ELSRoomPanelViewEnum.ReplayView : ELSRoomPanelViewEnum.PlayView;
            await yiuiRootComponent.OpenPanelAsync<LSRoomPanelComponent, ELSRoomPanelViewEnum>(viewIndex);

            await YIUIMgrComponent.Inst.ClosePanelAsync<LSLobbyPanelComponent>();
            await ETTask.CompletedTask;
        }
    }
}