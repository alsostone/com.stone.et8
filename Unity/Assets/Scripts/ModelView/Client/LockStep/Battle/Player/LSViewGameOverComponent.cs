using System.Collections.Generic;

namespace ET.Client
{
	[ComponentOf(typeof(LSUnitView))]
	public class LSViewSelectionComponent : Entity, IAwake, ILSRollback
	{
		public HashSet<long> SelectedIdSet = new();
	}
}