using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using ST.GridBuilder;

namespace ET
{
	[ComponentOf(typeof(LSWorld))]
	[MemoryPackable]
	public partial class LSGridMapComponent: LSEntity, IAwake<GridData>, IDeserialize, ISerializeToEntity
	{
		[MemoryPackIgnore]
		[BsonIgnore]
		public GridData GridData;
	}
}