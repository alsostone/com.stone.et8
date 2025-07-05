using System.Collections.Generic;

namespace ET
{
    // 客户端挂在Room 服务器挂在RoomPlayer
    [ComponentOf]
    public class LSCommandsComponent: Entity, IAwake<byte>
    {
        public byte SeatIndex { get; set; }
        
        public Queue<ulong> MoveCommands { get; } = new();
        public List<ulong> DragCommands { get; } = new();
        public List<ulong> Commands { get; } = new();
    }
}