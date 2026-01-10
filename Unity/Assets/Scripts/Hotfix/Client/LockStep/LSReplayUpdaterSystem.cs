using System;
using TrueSync;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSReplayUpdater))]
    [FriendOf(typeof(LSReplayUpdater))]
    public static partial class LSReplayUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this LSReplayUpdater self)
        {

        }
        
        [EntitySystem]
        private static void Update(this LSReplayUpdater self)
        {
            Room room = self.GetParent<Room>();
            if (room.LSWorld.EndFrame > 0 && room.AuthorityFrame >= room.LSWorld.EndFrame) {
                return;
            }
            
            long timeNow = TimeInfo.Instance.ServerNow();
            while (timeNow >= room.FixedTimeCounter.FrameTime(room.AuthorityFrame + 1))
            {
                if (room.AuthorityFrame + 1 >= room.Replay.FrameMessages.Count)
                    break;
                ++room.PredictionFrame;
                ++room.AuthorityFrame;
                Room2C_FrameMessage frameMessage = room.Replay.FrameMessages[room.AuthorityFrame];
                room.Update(frameMessage);

                long timeNow2 = TimeInfo.Instance.ServerNow();
                if (timeNow2 - timeNow > 20)
                    break;
            }
        }

        public static void ChangeReplaySpeed(this LSReplayUpdater self)
        {
            Room room = self.Room();
            LSReplayUpdater lsReplayUpdater = room.GetComponent<LSReplayUpdater>();
            if (lsReplayUpdater.ReplaySpeed == 8)
            {
                lsReplayUpdater.ReplaySpeed = 1;
            }
            else
            {
                lsReplayUpdater.ReplaySpeed *= 2;
            }

            FP updateInterval = LSConstValue.UpdateInterval / lsReplayUpdater.ReplaySpeed;
            room.FixedTimeCounter.ChangeInterval(updateInterval, room.AuthorityFrame);
        }
    }
}