using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TrackComponent : LSEntity, IAwake<int, LSUnit, TSVector>, ILSUpdate, ISerializeToEntity
    {
        public int TrackId;
        public long Target;
        public FP HorSpeed;
        
        public TSVector TargetPostion;
        public TSVector CasterPosition;
        public TSVector ControlPosition;
        
        public FP Duration;
        public FP EclipseTime;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbTrackRow TbTrackRow => this.tbTrackRow ?? TbTrack.Instance.Get(TrackId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbTrackRow tbTrackRow;
    }
}