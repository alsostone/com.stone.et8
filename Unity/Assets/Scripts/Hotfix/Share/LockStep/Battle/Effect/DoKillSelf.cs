namespace ET
{
	[EffectExecutor(EffectActionType.KillSelf)]
	public class DoKillSelf : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			owner?.GetComponent<DeathComponent>().DoDeath();
		}
	}
}
