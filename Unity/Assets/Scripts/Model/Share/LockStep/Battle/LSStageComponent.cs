using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
	[ComponentOf(typeof(LSWorld))]
	[MemoryPackable]
	public partial class LSStageComponent: LSEntity, IAwake<int>, ILSUpdate, ISerializeToEntity
	{
		public int TableId;
		public int CurrentWaveCount;
		public int CurrentMonsterCount;
		
		public int NextWaveFrame;
		
		[BsonIgnore]
		[MemoryPackIgnore]
		public TbStageRow TbRow => this.tbRow ?? TbStage.Instance.Get(this.TableId);
		[BsonIgnore]
		[MemoryPackIgnore]
		private TbStageRow tbRow;
	}
}