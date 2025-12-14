using TrueSync;

namespace ET
{
	[EffectExecutor(EffectActionType.Healing)]
	public class DoHealing : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			if (owner == null) { return; }	// 治疗必须依赖施法者 不然需要让子弹存储施法者属性 有需求再调整
			
			FP attack = FP.Zero;
			if (param.Length == 0) {
				attack = owner.GetComponent<PropComponent>().Get(NumericType.Atk);
			}
			else if (param[0] > 0) {
				attack = owner.GetComponent<PropComponent>().Get(NumericType.Atk);
				attack *= (FP)param[0] / LSConstValue.PropValueScale;
			}
			if (param.Length > 1) {
				attack += (FP)param[1] / LSConstValue.PropValueScale;
			}
			target.GetComponent<BeHitComponent>()?.BeHealing(owner, attack * count);
		}
	}
}
