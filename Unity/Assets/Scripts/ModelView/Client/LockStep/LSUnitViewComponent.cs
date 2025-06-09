namespace ET
{
	[ComponentOf(typeof(Room))]
	public class LSUnitViewComponent: Entity, IAwake, IDestroy, ILSRollback
	{
		public EntityRef<LSUnitView> myUnitView;
	}
}