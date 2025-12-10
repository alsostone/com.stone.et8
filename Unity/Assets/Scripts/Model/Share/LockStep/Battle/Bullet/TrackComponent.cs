using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TrackComponent : LSEntity, IAwake<int, int, int, FP>, IAwake<int, int, int, LSUnit>, IAwake<int, int, int, TSVector>, ILSUpdate, ISerializeToEntity
    {
        public FP HorSpeed;
        public ETrackTowardType TowardType;
        public TSVector CasterPosition;
        public TSVector TargetPosition;
        public FP ElapsedTime;
        
        public bool IsReached { set; get; }
        public FP ElapsedDistance => HorSpeed * ElapsedTime;
        
        // 跟随目标时使用
        public long Target;

        // 贝塞尔曲线相关
        public bool IsUesBezier;
        public TSVector ControlPosition;
        public FP Duration;
    }
}