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
		
		public List<FlowFieldNode[]> FlowFields;
		public Stack<int> FreeFlowField;
		public Dictionary<int, int> FlowFieldIndexRef;
		
		public int FlowFieldDefaultIndex;
		public bool FlowFieldDirty;
		public FieldV2 FlowFieldDestination;

		[MemoryPackIgnore, BsonIgnore]
		public GridData GridData;

		[MemoryPackIgnore, BsonIgnore]
		public TSVector GridPosition;
		
		[MemoryPackIgnore, BsonIgnore]
		public TSQuaternion GridRotation;
		
		[MemoryPackIgnore, BsonIgnore]
		public List<IndexV2> PathPoints = new ();
		
		[MemoryPackIgnore, BsonIgnore]
		public List<FieldV2> FlowFieldDestinations = new ();
		
	}
}