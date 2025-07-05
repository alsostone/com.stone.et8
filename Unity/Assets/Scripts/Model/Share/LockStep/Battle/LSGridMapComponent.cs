using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using ST.GridBuilder;

namespace ET
{
	[ComponentOf(typeof(LSWorld))]
	[MemoryPackable]
	public partial class LSGridMapComponent: LSEntity, IAwake<string>, IDeserialize, ISerializeToEntity
	{
		public string GridName;
		
		[MemoryPackIgnore]
		[BsonIgnore]
		public GridData GridData;
	}
}