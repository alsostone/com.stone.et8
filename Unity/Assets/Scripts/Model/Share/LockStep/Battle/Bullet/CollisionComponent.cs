using System.Collections.Generic;
using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class CollisionComponent : LSEntity, IAwake<int>, IDestroy, ISerializeToEntity
    {
        public int TestingInterval; // 碰撞检测的间隔帧数
        public int TestingFrame;    // 碰撞检测的帧数(用于避免每帧检测，提高性能)
        
        public TSVector PreviousPosition;
        public HashSet<long> HitSet = new (); // 已命中的目标列表，避免重复命中
    }
}