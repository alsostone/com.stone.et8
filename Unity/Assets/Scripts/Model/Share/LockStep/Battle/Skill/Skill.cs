using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(SkillComponent))]
    [MemoryPackable]
    public partial class Skill : LSEntity, IAwake<int, bool>, IDestroy, ILSUpdate, ISerializeToEntity
    {
        public int SkillId;
        public bool IsOnlyOnce;
        
        public int CastFrame;
        public int DurationFrame;
        public int CurrentPoint;
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public List<SearchUnit> SearchUnits = new List<SearchUnit>(16);
        [BsonIgnore]
        [MemoryPackIgnore]
        public TbSkillRow TbSkillRow => tbSkillRow ?? TbSkill.Instance.Get(this.SkillId);
        [BsonIgnore]
        [MemoryPackIgnore]
        private TbSkillRow tbSkillRow;
    }
}