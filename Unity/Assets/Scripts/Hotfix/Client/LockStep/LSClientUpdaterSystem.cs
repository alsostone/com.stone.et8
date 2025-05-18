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
                OneFrameInputs oneFrameInputs = self.GetOneFrameMessages(room.PredictionFrame);
                
                room.Update(oneFrameInputs);
                room.SendHash(room.PredictionFrame);
                
                room.SpeedMultiply = ++i;

                FrameMessage frameMessage = FrameMessage.Create();
                frameMessage.Frame = room.PredictionFrame;
                frameMessage.Input = self.Input;
                root.GetComponent<ClientSenderComponent>().Send(frameMessage);
                
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

        private static OneFrameInputs GetOneFrameMessages(this LSClientUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            if (frame <= room.AuthorityFrame)
            {
                return frameBuffer.FrameInputs(frame);
            }
            
            // predict
            OneFrameInputs predictionFrame = frameBuffer.FrameInputs(frame);
            
            frameBuffer.MoveForward(frame);
            if (frameBuffer.CheckFrame(room.AuthorityFrame))
            {
                OneFrameInputs authorityFrame = frameBuffer.FrameInputs(room.AuthorityFrame);
                authorityFrame.CopyTo(predictionFrame);
            }
            predictionFrame.Inputs[self.MyId] = self.Input;
            
            return predictionFrame;
        }
    }
}