using System.Collections.Generic;

namespace ET
{
    // 客户端挂在Room 服务器挂在RoomPlayer
    [ComponentOf]
    public class LSCommandsComponent: Entity, IAwake<byte>
    {
        public byte SeatIndex { get; set; }
        
        public List<Queue<ulong>> FramesCommandsMove { get; set; }
        public List<List<ulong>> FramesCommandsDrag { get; set; }
        public List<List<ulong>> FramesCommandsNormal { get; set; }
    }
}