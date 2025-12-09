using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TrackComponent : LSEntity, IAwake<int, FP>, IAwake<int, LSUnit>, IAwake<int, TSVector>, ILSUpdate, ISerializeToEntity
    {
        public int TrackId;
        public FP HorSpeed;
        public ETrackTowardType TowardType;
        public TSVector CasterPosition;
        public TSVector TargetPosition;
        public bool IsReached { set; get; }
        public FP ElapsedDistance => HorSpeed * ElapsedTime;
        
        // 跟随目标时使用
        public long Target;

        // 贝塞尔曲线相关
        public bool IsUesBezier;
        public TSVector ControlPosition;
        public FP Duration;
        public FP ElapsedTime;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbTrackRow TbTrackRow => this.tbTrackRow ?? TbTrack.Instance.Get(TrackId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbTrackRow tbTrackRow;
    }
}