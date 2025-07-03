using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class C2Room_TimeAdjustHandler: MessageHandler<Scene, C2Room_TimeAdjust>
    {
        protected override async ETTask Run(Scene root, C2Room_TimeAdjust message)
        {
            using C2Room_TimeAdjust _ = message;  // 让消息回到池中
            
            Room room = root.GetComponent<Room>();
            if (message.Frame % LSConstValue.FrameCountPerSecond == 0)
            {
                long nowFrameTime = room.FixedTimeCounter.FrameTime(message.Frame);
                int diffTime = (int)(nowFrameTime - TimeInfo.Instance.ServerFrameTime());

                Room2C_TimeAdjust timeAdjust = Room2C_TimeAdjust.Create(true);
                timeAdjust.DiffTime = diffTime;

                RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();
                RoomPlayer roomPlayer = roomServerComponent.GetRoomPlayer(message.SeatIndex);
                
                MessageLocationSenderOneType gateSession = room.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession);
                gateSession.Send(roomPlayer.Id, timeAdjust);
            }
            
            await ETTask.CompletedTask;
        }
    }
}