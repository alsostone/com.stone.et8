using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class LSInputComponent: LSEntity, ILSUpdate, IAwake, ISerializeToEntity
    {
        [BsonIgnore]
        [MemoryPackIgnore]
        public LSUnit Owner => this.GetParent<LSUnit>();
        
        public LSInput LSInput { get; set; }
    }
}