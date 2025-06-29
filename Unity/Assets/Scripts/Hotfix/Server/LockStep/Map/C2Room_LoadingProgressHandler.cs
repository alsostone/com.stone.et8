using System.Collections.Generic;
using TrueSync;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    [FriendOf(typeof (RoomServerComponent))]
    public class C2Room_LoadingProgressHandler: MessageHandler<Scene, C2Room_LoadingProgress>
    {
        protected override async ETTask Run(Scene root, C2Room_LoadingProgress message)
        {
            Room room = root.GetComponent<Room>();
            RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();
            RoomPlayer roomPlayer = roomServerComponent.GetChild<RoomPlayer>(message.PlayerId);
            roomPlayer.Progress = message.Progress;
            
            if (!roomServerComponent.IsAllPlayerProgress100())
            {
                return;
            }
            
            await room.Fiber.Root.GetComponent<TimerComponent>().WaitAsync(1000);

            Room2C_Start room2CStart = Room2C_Start.Create();
            room2CStart.StartTime = TimeInfo.Instance.ServerFrameTime();
            room.Start(room2CStart.StartTime);

            room.AddComponent<LSServerUpdater>();

            RoomMessageHelper.Broadcast(room, room2CStart);
        }
    }
}