using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class G2Room_ReconnectHandler: MessageHandler<Scene, G2Room_Reconnect, Room2G_Reconnect>
    {
        protected override async ETTask Run(Scene root, G2Room_Reconnect request, Room2G_Reconnect response)
        {
            Room room = root.GetComponent<Room>();

            int frame = room.AuthorityFrame;
            byte[] bytes = room.FrameBuffer.Snapshot(frame).ToArray();
            
            response.StartTime = room.StartTime;
            response.MatchInfo = room.Replay.MatchInfo;
            response.LSWorldBytes = bytes;
            
            await ETTask.CompletedTask;
        }
    }
}