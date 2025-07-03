using System;
using System.IO;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSClientUpdater))]
    [FriendOf(typeof(LSClientUpdater))]
    public static partial class LSClientUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this LSClientUpdater self)
        {
            Room room = self.GetParent<Room>();
            self.MyId = room.Root().GetComponent<PlayerComponent>().MyId;
            self.MySeatIndex = room.GetSeatIndex(self.MyId);
        }
        
        [EntitySystem]
        private static void Update(this LSClientUpdater self)
        {
            Room room = self.GetParent<Room>();
            long timeNow = TimeInfo.Instance.ServerNow();
            Scene root = room.Root();

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
                    root.GetComponent<ClientSenderComponent>().Send(timeAdjust);
                }
                
                Room2C_FrameMessage frameMessage = self.GetFrameMessage(room.PredictionFrame);
                
                room.Update(frameMessage);
                room.SendHash(room.PredictionFrame);
                
                room.SpeedMultiply = ++i;

                C2Room_FrameMessage sendFrameMessage = C2Room_FrameMessage.Create();
                sendFrameMessage.Frame = room.PredictionFrame;
                sendFrameMessage.Input = self.Input;
                root.GetComponent<ClientSenderComponent>().Send(sendFrameMessage);
                
                long timeNow2 = TimeInfo.Instance.ServerNow();
                if (timeNow2 - timeNow > 5)
                    break;
            }

            // 操作只要生效即清除 LSOperaComponentSystem会在下次需要生效前设置就绪
            // LSOperaComponentSystem的Update帧率一定大于等于该处生效的频率且保证触发在前 所以不会有空挡产生
            // 由于是生效后才清除 也不会出现操作被丢失的情况
            // 即使出现卡顿导致一次操作生效多次的情况 也是符合直觉的
            if (i > 0)
            {
                self.Input = new LSInput();
            }
        }

        private static Room2C_FrameMessage GetFrameMessage(this LSClientUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            Room2C_FrameMessage frameMessage = frameBuffer.GetFrameMessage(frame);
            frameBuffer.MoveForward(frame);
            
            // 若要获取的帧数据已经是服务器返回的直接用
            if (frame <= room.AuthorityFrame)
                return frameMessage;
            
            // 若没有服务器返回的帧数据 组织预测数据
            if (frameBuffer.CheckFrame(room.AuthorityFrame))
            {
                Room2C_FrameMessage authorityFrame = frameBuffer.GetFrameMessage(room.AuthorityFrame);
                authorityFrame.CopyTo(frameMessage);
            }
            frameMessage.Inputs[self.MySeatIndex] = self.Input;
            
            return frameMessage;
        }
    }
}