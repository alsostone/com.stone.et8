namespace ET
{
	[EffectExecutor(ESkillEffectType.AddBuff)]
	public class AddBuff : IEffectExecutor
	{
		public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			target.GetComponent<BuffComponent>().AddBuffs(param, owner);
		}
	}
}
