using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class FrameMessageHandler: MessageHandler<Scene, C2Room_FrameMessage>
    {
        protected override async ETTask Run(Scene root, C2Room_FrameMessage message)
        {
            using C2Room_FrameMessage _ = message;  // 让消息回到池中
            
            Room room = root.GetComponent<Room>();
            if (message.Frame % (1000 / LSConstValue.UpdateInterval) == 0)
            {
                long nowFrameTime = room.FixedTimeCounter.FrameTime(message.Frame);
                int diffTime = (int)(nowFrameTime - TimeInfo.Instance.ServerFrameTime());

                Room2C_AdjustUpdateTime room2CAdjustUpdateTime = Room2C_AdjustUpdateTime.Create();
                room2CAdjustUpdateTime.DiffTime = diffTime;
                room.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(message.PlayerId, room2CAdjustUpdateTime);
            }
            
            // 如果消息的帧数远大于服务器帧数则丢弃（用预测帧数量判定，其他不离谱的值也可）
            if (message.Frame > room.AuthorityFrame + LSConstValue.PredictionFrameMaxCount)
            {
                Log.Warning($"FrameMessage > AuthorityFrame + PredictionFrameMaxCount discard: {message}");
                return;
            }
            
            // 晚到的消息放在最近帧数据中 防止因丢操作影响手感
            int frame = Math.Max(room.AuthorityFrame + 1, message.Frame);
            Room2C_FrameMessage frameMessage = room.FrameBuffer.GetFrameMessage(frame);
            if (frameMessage.Inputs.TryGetValue(message.PlayerId, out LSInput value))
            {
                value.V = message.Input.V;                  // 覆盖 遥杆使用最新的
                value.Button |= message.Input.Button;       // 合并 防止因丢操作影响手感 32位代表32个按钮的按下状态
                frameMessage.Inputs[message.PlayerId] = value;
            }
            else
            {
                frameMessage.Inputs[message.PlayerId] = message.Input;
            }
            await ETTask.CompletedTask;
        }
    }
}