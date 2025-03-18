using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class SkillComponent : LSEntity, IAwake<List<int>>, IDestroy
    {
        [BsonIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, long> IdSkillMap = new Dictionary<int, long>();

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<ESkillType, List<long>> TypeSkillsMap = new Dictionary<ESkillType, List<long>>();

        public List<long> mRunningSkills = new List<long>();
    }
}