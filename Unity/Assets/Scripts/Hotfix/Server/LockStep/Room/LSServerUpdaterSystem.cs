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

            Room2C_FrameMessage sendFrameMessage = Room2C_FrameMessage.Create();
            frameMessage.CopyTo(sendFrameMessage);
            RoomMessageHelper.Broadcast(room, sendFrameMessage);

            room.Update(frameMessage);
        }

        private static Room2C_FrameMessage GetFrameMessage(this LSServerUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            Room2C_FrameMessage frameMessage = frameBuffer.GetFrameMessage(frame);
            frameBuffer.MoveForward(frame);
            
            frameMessage.Frame = frame;
            frameMessage.FrameIndex = frame;
            RoomServerComponent roomServerComponent = room.GetComponent<RoomServerComponent>();
            foreach (RoomPlayer roomPlayer in roomServerComponent.Children.Values)
            {
                LSCommandsComponent lsCommandsComponent = roomPlayer.GetComponent<LSCommandsComponent>();
                if (lsCommandsComponent.MoveCommands.Count > 0) {
                    frameMessage.Commands.Add(lsCommandsComponent.MoveCommands.Dequeue());
                }
                frameMessage.Commands.AddRange(lsCommandsComponent.DragCommands);
                lsCommandsComponent.DragCommands.Clear();
                frameMessage.Commands.AddRange(lsCommandsComponent.Commands);
                lsCommandsComponent.Commands.Clear();
            }
            
            return frameMessage;
        }
    }
}