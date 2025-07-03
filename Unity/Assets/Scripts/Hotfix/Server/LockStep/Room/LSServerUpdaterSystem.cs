using System;
using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(LSServerUpdater))]
    [FriendOf(typeof(LSServerUpdater))]
    public static partial class LSServerUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this LSServerUpdater self)
        {

        }
        
        [EntitySystem]
        private static void Update(this LSServerUpdater self)
        {
            Room room = self.GetParent<Room>();
            long timeNow = TimeInfo.Instance.ServerFrameTime();

            int frame = room.AuthorityFrame + 1;
            if (timeNow < room.FixedTimeCounter.FrameTime(frame))
            {
                return;
            }

            Room2C_FrameMessage frameMessage = self.GetFrameMessage(frame);
            ++room.AuthorityFrame;

            Room2C_FrameMessage sendInput = Room2C_FrameMessage.Create();
            frameMessage.CopyTo(sendInput);

            RoomMessageHelper.Broadcast(room, sendInput);

            room.Update(frameMessage);
        }

        private static Room2C_FrameMessage GetFrameMessage(this LSServerUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            Room2C_FrameMessage frameMessage = frameBuffer.GetFrameMessage(frame);
            frameBuffer.MoveForward(frame);

            if (frameMessage.Inputs.Count == LSConstValue.MatchCount)
            {
                return frameMessage;
            }

            Room2C_FrameMessage preFrameInputs = null;
            if (frameBuffer.CheckFrame(frame - 1))
            {
                preFrameInputs = frameBuffer.GetFrameMessage(frame - 1);
            }

            // 有人输入的消息没过来，给他使用上一帧的操作
            foreach (long playerId in room.PlayerIds)
            {
                if (frameMessage.Inputs.ContainsKey(playerId))
                {
                    continue;
                }

                if (preFrameInputs != null && preFrameInputs.Inputs.TryGetValue(playerId, out LSInput input))
                {
                    // 使用上一帧的输入
                    frameMessage.Inputs[playerId] = input;
                }
                else
                {
                    frameMessage.Inputs[playerId] = new LSInput();
                }
            }

            return frameMessage;
        }
    }
}