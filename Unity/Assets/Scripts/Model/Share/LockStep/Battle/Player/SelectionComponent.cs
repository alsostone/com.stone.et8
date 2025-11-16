using System.Collections.Generic;
using MemoryPack;

namespace ET
{
	[ComponentOf(typeof(LSUnit))]
	[MemoryPackable]
	public partial class SelectionComponent: LSEntity, IAwake, ISerializeToEntity
	{
		public List<long> SelectedUnitIds { get; set; }
	}
}