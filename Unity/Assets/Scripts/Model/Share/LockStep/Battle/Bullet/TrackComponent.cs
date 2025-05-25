using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TrackComponent : LSEntity, IAwake<int, LSUnit>, ILSUpdate, ISerializeToEntity
    {
        public int TrackId;
        public TSVector TargetPostion;
        public long Target;
        
        public int StartFrame;
        public FP HorSpeed;

        public FP VerSpeed;
        public FP VerAcceleration;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbTrackRow TbTrackRow => this.tbTrackRow ?? TbTrack.Instance.Get(TrackId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbTrackRow tbTrackRow;
    }
}