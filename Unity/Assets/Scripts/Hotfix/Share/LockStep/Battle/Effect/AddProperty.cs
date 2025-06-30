using TrueSync;

namespace ET
{
	[EffectExecutor(ESkillEffectType.AddProperty)]
	public class AddProperty : IEffectExecutor
	{
		public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			for (int i = 0; i < param.Length - 1; i+=2)
			{
				if (param[i] == 0) { continue; }
				NumericType type = (NumericType)param[i];
				FP value = param[i + 1] * FP.EN4;
				target.GetComponent<PropComponent>().Add(type, value);
			}
		}
	}
}
