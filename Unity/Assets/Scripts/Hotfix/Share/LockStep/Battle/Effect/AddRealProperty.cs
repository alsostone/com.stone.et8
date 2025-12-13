using TrueSync;

namespace ET
{
	[EffectExecutor(EffectActionType.ChangePropertyReal)]
	public class AddRealProperty : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			PropComponent propComponent = target.GetComponent<PropComponent>();
			for (int i = 0; i < param.Length - 1; i+=2)
			{
				if (param[i] == 0) { continue; }
				NumericType type = (NumericType)param[i];
				FP value = (FP)param[i + 1] / LSConstValue.PropValueScale;
				propComponent.AddRealProp(type, value * count);
			}
		}
	}
}
