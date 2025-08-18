using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class FieldMoveComponent : LSEntity, IAwake, ISerializeToEntity
    {
    }
}