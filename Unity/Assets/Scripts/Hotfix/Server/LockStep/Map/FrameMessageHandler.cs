using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class FrameMessageHandler: MessageHandler<Scene, FrameMessage>
    {
        protected override async ETTask Run(Scene root, FrameMessage message)
        {
            using FrameMessage _ = message;  // 让消息回到池中
            
            Room room = root.GetComponent<Room>();
            if (message.Frame % (1000 / LSConstValue.UpdateInterval) == 0)
            {
                long nowFrameTime = room.FixedTimeCounter.FrameTime(message.Frame);
                int diffTime = (int)(nowFrameTime - TimeInfo.Instance.ServerFrameTime());

                Room2C_AdjustUpdateTime room2CAdjustUpdateTime = Room2C_AdjustUpdateTime.Create();
                room2CAdjustUpdateTime.DiffTime = diffTime;
                room.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(message.PlayerId, room2CAdjustUpdateTime);
            }
            
            if (message.Frame > room.AuthorityFrame + BattleConst.PredictionFrameMaxCount)
            {
                Log.Warning($"FrameMessage > AuthorityFrame + PredictionFrameMaxCount discard: {message}");
                return;
            }
            
            // 晚到的消息放在最近帧数据中 防止因丢操作影响手感
            int frame = Math.Max(room.AuthorityFrame + 1, message.Frame);
            OneFrameInputs oneFrameInputs = room.FrameBuffer.FrameInputs(frame);
            if (oneFrameInputs.Inputs.TryGetValue(message.PlayerId, out LSInput value))
            {
                value.V = message.Input.V;                  // 遥杆使用最新的 直接覆盖
                value.Button |= message.Input.Button;       // 32位代表32个按钮的按下状态 合并他们
                oneFrameInputs.Inputs[message.PlayerId] = value;
            }
            else
            {
                oneFrameInputs.Inputs[message.PlayerId] = message.Input;
            }
            await ETTask.CompletedTask;
        }
    }
}