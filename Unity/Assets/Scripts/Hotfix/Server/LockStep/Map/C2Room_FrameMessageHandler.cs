using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class C2Room_FrameMessageHandler: MessageHandler<Scene, C2Room_FrameMessage>
    {
        protected override async ETTask Run(Scene root, C2Room_FrameMessage message)
        {
            using C2Room_FrameMessage _ = message;  // 让消息回到池中
            
            // 如果消息的帧数和服务器帧数差异过大则丢弃（用预测帧数量判定，其他不离谱的值也可）
            Room room = root.GetComponent<Room>();
            if (message.Frame < room.AuthorityFrame - LSConstValue.PredictionFrameMaxCount
                || message.Frame > room.AuthorityFrame + LSConstValue.PredictionFrameMaxCount) {
                Log.Warning($"The frame rate difference is too large. discard: {message}");
                return;
            }
            
            RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();
            RoomPlayer roomPlayer = roomServerComponent.GetRoomPlayer(message.SeatIndex);
            roomPlayer.GetComponent<LSCommandsComponent>().AddCommand(message.Command);

            await ETTask.CompletedTask;
        }
    }
}