using System.IO;
using TrueSync;

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
        
#if !DISABLE_FRAME_SNAPSHOT
        // 回滚
        public static void Rollback(Room room, int authorityFrame, int predictionFrame)
        {
            room.IsRollback = true;
            
            room.ProcessLog.SetLogEnable(false);
            room.LSWorld.Dispose();
            room.LSWorld = room.GetLSWorld(authorityFrame - 1);
            room.ProcessLog.SetLogEnable(true);
            
            for (int i = authorityFrame; i <= predictionFrame; ++i) {
                Room2C_FrameMessage frameMessage = room.FrameBuffer.GetFrameMessage(i);
                room.Update(frameMessage);
            }
            
            RunLSRollbackSystem(room);
            room.IsRollback = false;
        }
#endif
        private static bool RollbackReplay(Room room, int toFrame)
        {
            if (toFrame == room.AuthorityFrame) {
                return false;
            }

            int fromFrame = 0;
            if (room.Replay.Snapshots.Count > 0)
            {
                int index = toFrame / LSConstValue.SaveLSWorldFrameCount;
                fromFrame = index * LSConstValue.SaveLSWorldFrameCount + 1;
                room.IsRollback = true;
                room.ProcessLog.SetLogEnable(false);
                room.LSWorld.Dispose();
                byte[] bytes = room.Replay.Snapshots[index];
                room.LSWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), bytes, 0, bytes.Length) as LSWorld;
                room.ProcessLog.SetLogEnable(true);
            }
            else
            {
                // 没有快照不能回滚到之前的帧（因为需从零帧开始执行，耗时随帧数变化，暂无必要）
                if (toFrame < room.AuthorityFrame) {
                    return false;
                }
                fromFrame = room.AuthorityFrame + 1;
                room.IsRollback = true;
            }

            for (int i = fromFrame; i <= toFrame; ++i) {
                Room2C_FrameMessage frameMessage = room.Replay.FrameMessages[i];
                room.Update(frameMessage);
            }
            
            RunLSRollbackSystem(room);
            room.IsRollback = false;
            return true;
        }
        
        public static void SendHash(this Room self, int frame)
        {
            if (frame > self.AuthorityFrame) {
                return;
            }
            
            long hash = self.ProcessLog.GetFrameHash(frame);
            C2Room_CheckHash c2RoomCheckHash = C2Room_CheckHash.Create();
            c2RoomCheckHash.Frame = frame;
            c2RoomCheckHash.Hash = hash;
            self.Root().GetComponent<ClientSenderComponent>().Send(c2RoomCheckHash);
        }
        
        public static void SaveReplay(Room room, string path)
        {
            if (room.LockStepMode >= LockStepMode.Local) {
                return;
            }
            
            Log.Debug($"save replay: {path} frame: {room.Replay.FrameMessages.Count}");
            byte[] bytes = MemoryPackHelper.Serialize(room.Replay);
            File.WriteAllBytes(path, bytes);
        }
        
        public static void JumpReplay(Room room, int frame)
        {
            if (room.LockStepMode >= LockStepMode.Local) {
                return;
            }

            if (frame >= room.Replay.FrameMessages.Count)
            {
                frame = room.Replay.FrameMessages.Count - 1;
            }
            
            Log.Debug($"try jump replay to {frame}");
            if (RollbackReplay(room, frame))
            {
                room.AuthorityFrame = room.LSWorld.Frame;
                room.FixedTimeCounter.Reset(TimeInfo.Instance.ServerFrameTime() - (frame * LSConstValue.UpdateInterval).AsLong(), 0);
                Log.Debug($"jump replay to {frame} success");
            }
            else
            {
                Log.Debug($"jump replay to {frame} failed");
            }
        }
    }
}