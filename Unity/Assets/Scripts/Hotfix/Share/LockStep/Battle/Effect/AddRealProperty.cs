using TrueSync;

namespace ET
{
	[EffectExecutor(ESkillEffectType.ChangePropertyReal)]
	public class AddRealProperty : IEffectExecutor
	{
		public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			if (param.Length < 2) { return; }
			NumericType type = (NumericType)param[0];
			PropComponent propComponent = target.GetComponent<PropComponent>();
			propComponent.AddRealProp(type, param[1] * FP.EN4);
		}
	}
}
