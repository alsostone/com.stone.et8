using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
	[ComponentOf(typeof(LSWorld))]
	[MemoryPackable]
	public partial class LSTargetsComponent: LSEntity, IAwake, IDeserialize, ISerializeToEntity
	{
		[MemoryPackIgnore]
		[BsonIgnore]
		public Dictionary<TeamType, List<EntityRef<LSUnit>>> TeamLSUnitsMap = new Dictionary<TeamType, List<EntityRef<LSUnit>>>();
		
	}
}