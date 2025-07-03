using System.IO;

namespace ET.Client
{
    public static partial class LSClientHelper
    {
        public static void RunLSRollbackSystem(Entity entity)
        {
            if (entity is LSEntity)
            {
                return;
            }
            
            LSEntitySystemSingleton.Instance.LSRollback(entity);
            
            if (entity.Components.Count > 0)
            {
                foreach (var component in entity.Components)
                {
                    RunLSRollbackSystem(component);
                }
            }

            if (entity.Children.Count > 0)
            {
                foreach (var kv in entity.Children)
                {
                    RunLSRollbackSystem(kv.Value);
                }
            }
        }
        
        // 回滚
        public static void Rollback(Room room, int frame)
        {
            room.LSWorld.Dispose();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            // 回滚
            room.ProcessLog.SetLogEnable(false);
            room.LSWorld = room.GetLSWorld(frame - 1);
            room.ProcessLog.SetLogEnable(true);
            
            Room2C_FrameMessage authorityFrameMessage = frameBuffer.GetFrameMessage(frame);
            // 执行AuthorityFrame
            room.Update(authorityFrameMessage);
            room.SendHash(frame);

            
            // 重新执行预测的帧
            for (int i = room.AuthorityFrame + 1; i <= room.PredictionFrame; ++i)
            {
                Room2C_FrameMessage frameMessage = frameBuffer.GetFrameMessage(i);
                CopyOtherInputsTo(room, authorityFrameMessage, frameMessage); // 重新预测消息
                room.Update(frameMessage);
            }
            
            RunLSRollbackSystem(room);
        }
        
        public static void SendHash(this Room self, int frame)
        {
            if (frame > self.AuthorityFrame)
            {
                return;
            }
            long hash = self.FrameBuffer.GetHash(frame);
            C2Room_CheckHash c2RoomCheckHash = C2Room_CheckHash.Create();
            c2RoomCheckHash.Frame = frame;
            c2RoomCheckHash.Hash = hash;
            self.Root().GetComponent<ClientSenderComponent>().Send(c2RoomCheckHash);
        }
        
        // 重新调整预测消息，只需要调整其他玩家的输入
        public static void CopyOtherInputsTo(Room room, Room2C_FrameMessage from, Room2C_FrameMessage to)
        {
            int mySeatIndex = room.GetComponent<LSClientUpdater>().MySeatIndex;
            foreach (var kv in from.Inputs)
            {
                if (kv.Key == mySeatIndex)
                {
                    continue;
                }
                to.Inputs[kv.Key] = kv.Value;
            }
        }

        public static void SaveReplay(Room room, string path)
        {
            if (room.IsReplay)
            {
                return;
            }
            
            Log.Debug($"save replay: {path} frame: {room.Replay.FrameMessages.Count}");
            byte[] bytes = MemoryPackHelper.Serialize(room.Replay);
            File.WriteAllBytes(path, bytes);
        }
        
        public static void JumpReplay(Room room, int frame)
        {
            if (!room.IsReplay)
            {
                return;
            }

            if (frame >= room.Replay.FrameMessages.Count)
            {
                frame = room.Replay.FrameMessages.Count - 1;
            }
            
            int snapshotIndex = frame / LSConstValue.SaveLSWorldFrameCount;
            Log.Debug($"jump replay start {room.AuthorityFrame} {frame} {snapshotIndex}");
            if (snapshotIndex != room.AuthorityFrame / LSConstValue.SaveLSWorldFrameCount || frame < room.AuthorityFrame)
            {
                room.LSWorld.Dispose();
                // 回滚
                byte[] memoryBuffer = room.Replay.Snapshots[snapshotIndex];
                LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), memoryBuffer, 0, memoryBuffer.Length) as LSWorld;
                room.LSWorld = lsWorld;
                room.AuthorityFrame = snapshotIndex * LSConstValue.SaveLSWorldFrameCount;
                RunLSRollbackSystem(room);
            }
            
            room.FixedTimeCounter.Reset(TimeInfo.Instance.ServerFrameTime() - frame * LSConstValue.UpdateInterval, 0);

            Log.Debug($"jump replay finish {frame}");
        }
    }
}