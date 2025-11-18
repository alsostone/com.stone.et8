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
		public GridData GridData;
		public TSVector GridPosition;
		public TSQuaternion GridRotation;

		public List<FlowFieldNode[]> FlowFields;
		public Stack<int> FreeFlowField;
		public Dictionary<int, int> FlowFieldIndexRef;
		
		public int FlowFieldDefaultIndex;
		public bool FlowFieldDirty;
		public FieldV2 FlowFieldDestination;

		[MemoryPackIgnore, BsonIgnore]
		public List<IndexV2> PathPoints = new ();
		
		[MemoryPackIgnore, BsonIgnore]
		public List<FieldV2> FlowFieldDestinations = new ();
		
	}
}