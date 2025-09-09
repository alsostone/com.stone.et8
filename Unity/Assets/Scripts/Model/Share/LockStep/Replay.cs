using System.Collections.Generic;
using MemoryPack;

namespace ET
{
    [MemoryPackable]
    public partial class Replay: Object
    {
        [MemoryPackOrder(1)]
        public LockStepMatchInfo MatchInfo;
        
        [MemoryPackOrder(2)]
        public List<Room2C_FrameMessage> FrameMessages = new();
        
        [MemoryPackOrder(3)]
        public List<byte[]> Snapshots = new();
        
        [MemoryPackOrder(4)]
        public byte[] LSWorldBytes; // 重连的战斗 MatchInfo为空 需使用LSWorldBytes
        
        [MemoryPackOrder(5)]
        public long OwnerPlayerId; // 该录像的拥有者 无拥有者则为0
    }
}