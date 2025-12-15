using System;

namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class Room2C_FrameMessageHandler: MessageHandler<Scene, Room2C_FrameMessage>
    {
        protected override async ETTask Run(Scene root, Room2C_FrameMessage message)
        {
            using Room2C_FrameMessage _ = message;  // 让消息回到池中
            
            Room room = root.GetComponent<Room>();
            if (room.LockStepMode <= LockStepMode.Local) {
                Log.Error("frame message error. in non-networked mode");
                return;
            }
            if (message.FrameIndex != room.AuthorityFrame + 1) {
                Log.Error($"frame message error. out of order frame {message.FrameIndex} != {room.AuthorityFrame} + 1");
            }
            
            ++room.AuthorityFrame;
            FrameBuffer frameBuffer = room.FrameBuffer;
            
#if !DISABLE_FRAME_SNAPSHOT
            // 服务端返回的消息比预测的还早
            if (room.AuthorityFrame > room.PredictionFrame)
            {
                Room2C_FrameMessage frameMessage = frameBuffer.GetFrameMessage(room.AuthorityFrame);
                message.CopyTo(frameMessage);
            }
            else
            {
                // 服务端返回来的消息，跟预测消息对比
                Room2C_FrameMessage predictionFrameMessage = frameBuffer.GetFrameMessage(room.AuthorityFrame);
                // 对比失败有两种可能，
                // 1是别人的输入预测失败，这种很正常，
                // 2自己的输入对比失败，这种情况是自己发送的消息比服务器晚到了，服务器使用了你的上一次输入
                // 回滚重新预测的时候，自己的输入不用变化
                if (!message.Equals(predictionFrameMessage))
                {
                    Log.Debug($"frame diff: {room.AuthorityFrame} {predictionFrameMessage} {message}");
                    Room2C_FrameMessage nextFrameMessage = frameBuffer.GetFrameMessage(room.AuthorityFrame + 1);
                    predictionFrameMessage.InsertTo(nextFrameMessage, message);
                    message.CopyTo(predictionFrameMessage);

                    Log.Debug($"roll back start {room.AuthorityFrame}");
                    LSClientHelper.Rollback(room, room.AuthorityFrame, room.PredictionFrame);
                    Log.Debug($"roll back finish {room.PredictionFrame}");
                }
                else // 对比成功
                {
                    room.Record();
                }
                room.SendHash(room.AuthorityFrame);
            }
#else
            Room2C_FrameMessage frameMessage = frameBuffer.GetFrameMessage(room.AuthorityFrame);
            message.CopyTo(frameMessage);
#endif
            await ETTask.CompletedTask;
        }
    }
}