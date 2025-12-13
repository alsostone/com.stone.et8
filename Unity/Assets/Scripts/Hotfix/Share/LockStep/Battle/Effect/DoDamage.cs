using TrueSync;

namespace ET
{
	[EffectExecutor(EffectActionType.Damage)]
	public class DoDamage : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			if (owner == null) { return; }	// 伤害必须依赖攻击者 不然需要让子弹存储攻击者属性 按需求调整即可
			FP attack = owner.GetComponent<PropComponent>().Get(NumericType.Atk);
			if (param.Length > 0)
			{
				attack *= (FP)param[0] / LSConstValue.PropValueScale;
			}
			attack += param.Length > 1 ? (FP)param[1] / LSConstValue.PropValueScale : 0;
			target.GetComponent<BeHitComponent>()?.BeDamage(owner, attack * count);
		}
	}
}
