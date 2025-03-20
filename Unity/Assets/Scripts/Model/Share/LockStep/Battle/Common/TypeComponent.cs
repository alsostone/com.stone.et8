using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    public class TypeComponent : LSEntity, IAwake<EUnitType>
    {
        public EUnitType Type;
    }
}