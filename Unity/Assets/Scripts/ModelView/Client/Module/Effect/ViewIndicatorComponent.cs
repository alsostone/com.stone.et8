namespace ET.Client
{
	[ComponentOf]
	public class ViewIndicatorComponent : Entity, IAwake<int, float>, IDestroy
	{
		public int RangeIndicator;
		public float AttackRange;
	}
}