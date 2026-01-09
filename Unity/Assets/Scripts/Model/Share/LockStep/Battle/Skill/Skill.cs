using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [MemoryPackable]
    public partial struct SearchUnitPackable
    {
        public long Target;
        public FP SqrDistance;
    }
    
    [ChildOf(typeof(SkillComponent))]
    [MemoryPackable]
    public partial class Skill : LSEntity, IAwake<int, bool>, IDestroy, ISerializeToEntity
    {
        public int SkillId;
        public bool IsOnlyOnce;
        public bool IsRunning { get; set; }
        
        public FP StartTime;
        public FP DurationTime;
        public int CurrentPoint;
        
        public List<SearchUnitPackable> SearchUnits;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbSkillRow TbSkillRow => tbSkillRow ?? TbSkill.Instance.Get(this.SkillId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbSkillRow tbSkillRow;
    }
}