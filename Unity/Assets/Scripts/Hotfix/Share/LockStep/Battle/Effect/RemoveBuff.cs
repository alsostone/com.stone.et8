namespace ET
{
	[EffectExecutor(EffectActionType.RemoveBuff)]
	public class RemoveBuff : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			target.GetComponent<BuffComponent>().RemoveBuffs(param);
		}
	}
}
