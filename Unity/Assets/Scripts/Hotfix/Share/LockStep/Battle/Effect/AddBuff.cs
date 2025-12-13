namespace ET
{
	[EffectExecutor(EffectActionType.AddBuff)]
	public class AddBuff : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			target.GetComponent<BuffComponent>().AddBuffs(param, owner);
		}
	}
}
