namespace ET
{
	[EffectExecutor(EffectActionType.RemoveRestrict)]
	public class RemoveRestrict : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			if (param.Length == 0) { return; }
			FlagComponent flagComponent = target.GetComponent<FlagComponent>();
			flagComponent.RemoveRestrict(param[0], count);
		}
	}
}
