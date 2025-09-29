using TrueSync;

namespace ET
{
	[EffectExecutor(ESkillEffectType.AddBulletPosition)]
	public class AddBulletPosition : IEffectExecutor
	{
		public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			var ownerTransform = owner.GetComponent<TransformComponent>();
			var position = TSVector.zero;
			if (param.Length >= 4) {
				position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
			}else if (param.Length >= 3) {
				position = new TSVector(param[1], param[2], 0) * FP.EN4;
			}else if (param.Length >= 2) {
				position = new TSVector(param[1], 0, 0) * FP.EN4;
			}
			position = ownerTransform.TransformPoint(position);
			TSVector targetPosition = target?.GetComponent<TransformComponent>()?.Position ?? TSVector.zero;
			LSUnitFactory.CreateBulletToPosition(owner.LSWorld(), param[0], position, owner, targetPosition);
		}
	}
}
