using TrueSync;

namespace ET
{
	[EffectExecutor(ESkillEffectType.Healing)]
	public class DoHealing : IEffectExecutor
	{
		public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			FP attack = owner.GetComponent<PropComponent>().Get(NumericType.Atk);
			if (param.Length > 0)
			{
				attack *= param[0] * FP.EN4;
			}
			attack += param.Length > 1 ? param[1] * FP.EN4 : 0;
			target.GetComponent<BeHitComponent>()?.BeHealing(owner, attack);
		}
	}
}
