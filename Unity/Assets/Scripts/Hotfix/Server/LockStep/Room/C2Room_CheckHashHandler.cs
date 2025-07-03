using System.IO;
using ICSharpCode.SharpZipLib.BZip2;

namespace ET.Server
{
    [MessageHandler(SceneType.RoomRoot)]
    public class C2Room_CheckHashHandler: MessageHandler<Scene, C2Room_CheckHash>
    {
        protected override async ETTask Run(Scene root, C2Room_CheckHash message)
        {
            Room room = root.GetComponent<Room>();
            if (room.IsInconsistent)
                return;
            
            long hash = room.FrameBuffer.GetHash(message.Frame);
            if (message.Hash != hash)
            {
                room.IsInconsistent = true;

                using var stream = room.ProcessLog.GetLogStream();
                using var ms = new MemoryStream();
                BZip2.Compress(stream, ms, false, 6);
                
                byte[] bytes = room.FrameBuffer.Snapshot(message.Frame).ToArray();
                Room2C_CheckHashFail room2CCheckHashFail = Room2C_CheckHashFail.Create();
                room2CCheckHashFail.Frame = message.Frame;
                room2CCheckHashFail.LSWorldBytes = bytes;
                room2CCheckHashFail.LSProcessBytes = ms.ToArray();

                MessageLocationSenderOneType gateSession = room.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession);
                gateSession.Send(room.PlayerIds[message.SeatIndex], room2CCheckHashFail);
            }
            await ETTask.CompletedTask;
        }
    }
}