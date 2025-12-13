namespace ET
{
	[EffectExecutor(EffectActionType.AddRestrict)]
	public class AddRestrict : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			if (param.Length == 0) { return; }
			FlagComponent flagComponent = target.GetComponent<FlagComponent>();
			flagComponent.AddRestrict(param[0], count);
		}
	}
}
