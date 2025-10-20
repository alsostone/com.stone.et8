using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using ST.GridBuilder;
using TrueSync;
using System.Collections.Generic;

namespace ET
{
	[ComponentOf(typeof(LSWorld))]
	[MemoryPackable]
	public partial class LSGridMapComponent: LSEntity, IAwake<string>, ILSUpdate, IDeserialize, ISerializeToEntity
	{
		public string GridName;
		
		[MemoryPackIgnore]
		[BsonIgnore]
		public GridData GridData { get; set; }

		[MemoryPackIgnore]
		[BsonIgnore]
		public TSVector GridPosition;
		
		[MemoryPackIgnore]
		[BsonIgnore]
		public TSQuaternion GridRotation;
		
		[MemoryPackIgnore]
		[BsonIgnore]
		public List<IndexV2> PathPoints;
	}
}