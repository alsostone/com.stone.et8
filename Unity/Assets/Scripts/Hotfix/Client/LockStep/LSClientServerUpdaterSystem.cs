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
            while (timeNow >= room.FixedTimeCounter.FrameTime(room.PredictionFrame + 1))
            {
                if (room.PredictionFrame - room.AuthorityFrame > LSConstValue.PredictionFrameMaxCount)
                    break;
                ++room.PredictionFrame;
                
#if !DISABLE_FRAME_SNAPSHOT
                // 每秒发送一次时间对齐消息 根据网络延迟调整时间间隔 尽量确保客户端处于：服务器绝对时间+网络延迟的时刻
                // 以此保证公平性 玩家不会因为网络问题而处于落后状态 但快照消耗较大，适合战场数据量较少的游戏
                if (room.PredictionFrame % LSConstValue.FrameCountPerSecond == 0)
                {
                    C2Room_TimeAdjust timeAdjust = C2Room_TimeAdjust.Create(true);
                    timeAdjust.Frame = room.PredictionFrame;
                    room.Root().GetComponent<ClientSenderComponent>().Send(timeAdjust);
                }

                Room2C_FrameMessage frameMessage = self.GetFrameMessage(room.PredictionFrame);
                room.Update(frameMessage);
                room.SendHash(room.PredictionFrame);
#else
                // 以最新接收的权威帧为基准 调整时间间隔 若预测帧过多则增大间隔以降低预测帧数
                // 网络延迟越高画面越滞后 但由于预测的帧数少且只有小范围回滚（移动）性能有保障 网络差公平性低也较符合预期
                int interval = LSConstValue.UpdateInterval;
                interval += room.PredictionFrame - room.AuthorityFrame;
                room.FixedTimeCounter.ChangeInterval(Math.Clamp(interval, 40, 66), room.PredictionFrame);
                
                int toFrame = Math.Min(room.AuthorityFrame, room.PredictionFrame);
                for (int frame = room.LSWorld.Frame + 1; frame <= toFrame; ++frame)
                {
                    Room2C_FrameMessage frameMessage = self.GetFrameMessage(frame);
                    room.Update(frameMessage);
                    room.SendHash(frame);
                }
#endif
                long timeNow2 = TimeInfo.Instance.ServerNow();
                if (timeNow2 - timeNow > 20)
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