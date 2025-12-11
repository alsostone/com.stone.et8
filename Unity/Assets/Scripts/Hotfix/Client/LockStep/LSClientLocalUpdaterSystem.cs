using System;
using System.IO;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSClientLocalUpdater))]
    [FriendOf(typeof(LSClientLocalUpdater))]
    public static partial class LSClientLocalUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this LSClientLocalUpdater self)
        {
        }
        
        [EntitySystem]
        private static void Update(this LSClientLocalUpdater self)
        {
            Room room = self.GetParent<Room>();
            if (room.LSWorld.EndFrame > 0 && room.AuthorityFrame >= room.LSWorld.EndFrame) {
                return;
            }
            
            long timeNow = TimeInfo.Instance.ServerNow();
            
            while (timeNow >= room.FixedTimeCounter.FrameTime(room.AuthorityFrame + 1))
            {
                ++room.PredictionFrame;
                ++room.AuthorityFrame;
                Room2C_FrameMessage frameMessage = self.GetFrameMessage(room.AuthorityFrame);
                
                room.Update(frameMessage);
                
                long timeNow2 = TimeInfo.Instance.ServerNow();
                if (timeNow2 - timeNow > 20)
                    break;
            }
        }

        private static Room2C_FrameMessage GetFrameMessage(this LSClientLocalUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            Room2C_FrameMessage frameMessage = frameBuffer.GetFrameMessage(frame);
            frameBuffer.MoveForward(frame);
            
            // 若没有服务器返回的帧数据 组织预测数据
            frameMessage.Frame = frame;
            frameMessage.FrameIndex = frame;
            
            LSCommandsComponent lsCommandsComponent = room.GetComponent<LSCommandsComponent>();
            lsCommandsComponent.AppendToFrameMessage(frame, frameMessage);
            
            return frameMessage;
        }
    }
}