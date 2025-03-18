using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf(typeof(SkillComponent))]
    public class Skill : LSEntity, IAwake<int, bool>, IDestroy, ILSUpdate
    {
        [BsonIgnore]
        public LSUnit Owner => this.GetParent<SkillComponent>().Owner;

        public int SkillId;
        public int Duration;
        public long CastTime;
        public int CurrentPoint;
        public bool IsOnlyOnce;
        
        [BsonIgnore]
        public List<SearchUnit> SearchUnits = new List<SearchUnit>(16);
        [BsonIgnore]
        public TbSkillRow TbSkillRow => tbSkillRow ?? TbSkill.Instance.Get(this.SkillId);
        [BsonIgnore]
        private TbSkillRow tbSkillRow;
    }
}