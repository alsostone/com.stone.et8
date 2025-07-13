using System.Collections.Generic;

namespace ET
{
	[ComponentOf(typeof(Room))]
	public class LSUnitViewComponent: Entity, IAwake, IDestroy, ILSRollback
	{
		public List<EntityRef<LSUnitView>> PlayerViews { get; } = new ();
	}
}