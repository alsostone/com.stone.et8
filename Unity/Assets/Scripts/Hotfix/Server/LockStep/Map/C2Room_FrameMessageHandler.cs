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
            
            Room room = root.GetComponent<Room>();

            // 如果消息的帧数远大于服务器帧数则丢弃（用预测帧数量判定，其他不离谱的值也可）
            if (message.Frame > room.AuthorityFrame + LSConstValue.PredictionFrameMaxCount)
            {
                Log.Warning($"FrameMessage > AuthorityFrame + PredictionFrameMaxCount discard: {message}");
                return;
            }
            
            // 晚到的消息放在最近帧数据中 防止因丢操作影响手感
            int frame = Math.Max(room.AuthorityFrame + 1, message.Frame);
            Room2C_FrameMessage frameMessage = room.FrameBuffer.GetFrameMessage(frame);
            if (frameMessage.Inputs.TryGetValue(message.SeatIndex, out LSInput value))
            {
                value.V = message.Input.V;                  // 覆盖 遥杆使用最新的
                value.Button |= message.Input.Button;       // 合并 防止因丢操作影响手感 32位代表32个按钮的按下状态
                frameMessage.Inputs[message.SeatIndex] = value;
            }
            else
            {
                frameMessage.Inputs[message.SeatIndex] = message.Input;
            }
            await ETTask.CompletedTask;
        }
    }
}