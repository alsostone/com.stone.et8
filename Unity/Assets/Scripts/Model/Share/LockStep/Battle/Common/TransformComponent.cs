using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TransformComponent : LSEntity, IAwake<TSVector, TSQuaternion>, ILSUpdate, ISerializeToEntity
    {
        public bool IsMovingCurrent;
        public bool IsMovingPrevious;
        public TSVector Position { get; set; }

        [MemoryPackIgnore]
        [BsonIgnore]
        public TSVector Forward
        {
            get => this.Rotation * TSVector.forward;
            set => this.Rotation = TSQuaternion.LookRotation(value, TSVector.up);
        }
        
        public TSQuaternion Rotation
        {
            get;
            set;
        }
    }
}