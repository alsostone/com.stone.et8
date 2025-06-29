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
        public List<OneFrameInputs> FrameInputs = new();
        
        [MemoryPackOrder(3)]
        public List<byte[]> Snapshots = new();
        
        [MemoryPackOrder(4)]
        public byte[] LSWorldBytes; // 重连的战斗 MatchInfo为空 需使用LSWorldBytes
    }
}