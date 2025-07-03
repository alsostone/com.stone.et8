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
            using C2Room_LoadingProgress _ = message;  // 让消息回到池中
            
            Room room = root.GetComponent<Room>();
            RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();
            RoomPlayer roomPlayer = roomServerComponent.GetRoomPlayer(message.SeatIndex);
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