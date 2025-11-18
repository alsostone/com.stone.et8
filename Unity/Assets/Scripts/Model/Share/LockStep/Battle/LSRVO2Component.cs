using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using RVO;

namespace ET
{
	[ComponentOf(typeof(LSWorld))]
	[MemoryPackable]
	public partial class LSRVO2Component: LSEntity, IAwake, ILSUpdate, IDestroy, IDeserialize, ISerializeToEntity
	{
		[MemoryPackIgnore]
		[BsonIgnore]
		public Simulator RVO2Simulator;
	}
}