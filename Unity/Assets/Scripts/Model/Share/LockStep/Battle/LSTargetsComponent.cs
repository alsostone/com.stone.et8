using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using ST.Mono;

namespace ET
{
	[ComponentOf(typeof(LSWorld))]
	[MemoryPackable]
	public partial class LSTargetsComponent: LSEntity, IAwake, ISerializeToEntity
	{
		[BsonIgnore]
		public Dictionary<TeamType, DynamicTree<long>> TeamLSUnitsMap = new ();
		
	}
}