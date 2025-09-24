using System.Collections.Generic;

namespace ET
{
    // 客户端挂在Room 服务器挂在RoomPlayer
    [ComponentOf]
    public class LSCommandsComponent: Entity, IAwake<byte>
    {
        public byte SeatIndex { get; set; }
        
        public List<Queue<LSCommandData>> FramesCommandsMove { get; set; }
        public List<List<LSCommandData>> FramesCommandsDrag { get; set; }
        public List<List<LSCommandData>> FramesCommandsNormal { get; set; }
    }
}