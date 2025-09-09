using System;

namespace ET.Client
{
    [Event(SceneType.LockStep)]
    public class LSSceneInitFinish_Finish: AEvent<Scene, LSSceneInitFinish>
    {
        protected override async ETTask Run(Scene clientScene, LSSceneInitFinish args)
        {
            Room room = clientScene.GetComponent<Room>();
            
            room.AddComponent<LSViewGridMapComponent>();
            room.AddComponent<LSUnitViewComponent>();
            room.AddComponent<LSCameraComponent>();

            switch (room.LockStepMode)
            {
                case LockStepMode.ObserverFile:
                case LockStepMode.ObserverReport:
                {
                    room.AddComponent<LSReplayUpdater>();
                    break;
                }
                case LockStepMode.Local:
                case LockStepMode.Server:
                {
                    room.AddComponent<LSCommandsComponent, byte>((byte)room.GetOwnerSeatIndex());
                    room.AddComponent<LSOperaComponent>();
                    room.AddComponent<LSOperaDragComponent>();
                    room.AddComponent<LSClientUpdater>();
                    break;
                }
                case LockStepMode.Observer:
                {
                    room.AddComponent<LSClientUpdater>();
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