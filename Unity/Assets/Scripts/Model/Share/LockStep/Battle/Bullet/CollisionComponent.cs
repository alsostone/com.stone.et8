using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CollisionComponent : LSEntity, IAwake<int, int>, ILSUpdate, ISerializeToEntity
    {
        public int SearchID;      // 碰撞检测时的索敌ID
        public int TestingInterval; // 碰撞检测的间隔帧数
        
        public TSVector PreviousPosition;
        public int TestingFrame;    // 碰撞检测的帧数(用于避免每帧检测，提高性能)
    }
}