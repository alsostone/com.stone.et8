using TrueSync;

namespace ET
{
	[EffectExecutor(ESkillEffectType.Damage)]
	public class DoDamage : IEffectExecutor
	{
		public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			FP attack = owner.GetComponent<PropComponent>().Get(NumericType.Atk);
			if (param.Length > 0)
			{
				attack *= (FP)param[0] / LSConstValue.PropValueScale;
			}
			attack += param.Length > 1 ? (FP)param[1] / LSConstValue.PropValueScale : 0;
			target.GetComponent<BeHitComponent>()?.BeDamage(owner, attack);
		}
	}
}
