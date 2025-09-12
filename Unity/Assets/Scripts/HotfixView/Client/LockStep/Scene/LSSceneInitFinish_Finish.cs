using System;

namespace ET.Client
{
    [Event(SceneType.LockStep)]
    public class LSSceneInitFinish_Finish: AEvent<Scene, LSSceneInitFinish>
    {
        protected override async ETTask Run(Scene clientScene, LSSceneInitFinish args)
        {
            Room room = clientScene.GetComponent<Room>();
            
            room.AddComponent<LSCameraComponent>();
            room.AddComponent<LSViewGameOverComponent>();
            room.AddComponent<LSViewGridMapComponent>();
            room.AddComponent<LSViewGridBuilderComponent>();
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

            await YIUIMgrComponent.Inst.ClosePanelAsync<LSLobbyPanelComponent>();
            await ETTask.CompletedTask;
        }
    }
}