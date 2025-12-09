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
        
        public TSVector Upwards { get; set; }
        public TSVector Position { get; set; }
        public TSVector2 RVO2Velocity { get; set; }  // RVO2当前速度，回滚时需要使用故需缓存
        public TSVector2 RVO2PrefVelocity { get; set; } // RVO2期望速度，回滚时需要使用故需缓存

        [MemoryPackIgnore]
        [BsonIgnore]
        public TSVector Forward
        {
            get => this.Rotation * TSVector.forward;
            set => this.Rotation = TSQuaternion.LookRotation(value, Upwards);
        }
        
        public TSQuaternion Rotation
        {
            get;
            set;
        }
    }
}