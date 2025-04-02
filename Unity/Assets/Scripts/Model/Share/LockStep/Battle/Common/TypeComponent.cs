using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class TypeComponent : LSEntity, IAwake<EUnitType>, ISerializeToEntity
    {
        public EUnitType Type;
    }
}