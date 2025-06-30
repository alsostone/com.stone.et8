namespace ET
{
	[EffectExecutor(ESkillEffectType.AddRestrict)]
	public class AddRestrict : IEffectExecutor
	{
		public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			if (param.Length == 0) { return; }
			FlagComponent flagComponent = target.GetComponent<FlagComponent>();
			flagComponent.AddRestrict(param[0]);
		}
	}
}
