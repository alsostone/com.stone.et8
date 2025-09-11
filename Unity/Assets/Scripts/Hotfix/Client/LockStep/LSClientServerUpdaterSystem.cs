using System;
using System.IO;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSClientServerUpdater))]
    [FriendOf(typeof(LSClientServerUpdater))]
    public static partial class LSClientServerUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this LSClientServerUpdater self)
        {
        }
        
        [EntitySystem]
        private static void Update(this LSClientServerUpdater self)
        {
            Room room = self.GetParent<Room>();
            if (room.LSWorld.EndFrame > 0 && room.AuthorityFrame >= room.LSWorld.EndFrame) {
                return;
            }

            long timeNow = TimeInfo.Instance.ServerNow();

            int i = 0;
            while (timeNow >= room.FixedTimeCounter.FrameTime(room.PredictionFrame + 1))
            {
                if (room.PredictionFrame - room.AuthorityFrame > LSConstValue.PredictionFrameMaxCount)
                    break;

                ++room.PredictionFrame;
                if (room.PredictionFrame % LSConstValue.FrameCountPerSecond == 0)
                {
                    C2Room_TimeAdjust timeAdjust = C2Room_TimeAdjust.Create(true);
                    timeAdjust.Frame = room.PredictionFrame;
                    room.Root().GetComponent<ClientSenderComponent>().Send(timeAdjust);
                }
                
                Room2C_FrameMessage frameMessage = self.GetFrameMessage(room.PredictionFrame);
                
                room.Update(frameMessage);
                room.SendHash(room.PredictionFrame);
                
                room.SpeedMultiply = ++i;
                
                long timeNow2 = TimeInfo.Instance.ServerNow();
                if (timeNow2 - timeNow > 5)
                    break;
            }
        }

        private static Room2C_FrameMessage GetFrameMessage(this LSClientServerUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            Room2C_FrameMessage frameMessage = frameBuffer.GetFrameMessage(frame);
            frameBuffer.MoveForward(frame);
            
            // 若要获取的帧数据已经是服务器返回的直接用
            if (frame <= room.AuthorityFrame)
                return frameMessage;
            
            // 若没有服务器返回的帧数据 组织预测数据
            frameMessage.Frame = frame;
            frameMessage.FrameIndex = frame;
            
            LSCommandsComponent lsCommandsComponent = room.GetComponent<LSCommandsComponent>();
            lsCommandsComponent.AppendToFrameMessage(frame, frameMessage);
            
            return frameMessage;
        }
    }
}