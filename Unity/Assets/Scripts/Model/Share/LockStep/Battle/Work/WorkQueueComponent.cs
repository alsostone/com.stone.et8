using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
	[ComponentOf(typeof(LSUnit))]
	[MemoryPackable]
	public partial class WorkQueueComponent: LSEntity, IAwake, ILSUpdate, ISerializeToEntity
	{
		public int WorkerCount;

		[MemoryPackIgnore]
		[BsonIgnore]
		public List<EntityRef<WorkComponent>> WorkComponents { get; } = new ();
	}
}