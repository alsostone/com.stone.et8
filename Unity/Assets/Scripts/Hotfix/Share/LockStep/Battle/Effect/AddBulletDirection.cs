using TrueSync;

namespace ET
{
	[EffectExecutor(EffectActionType.AddBulletDirection)]
	public class AddBulletDirection : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			// 子弹ID,x,y,z,Y轴发射角度,索敌ID
			if (param.Length < 6) { return; }
			
			TransformComponent targetTransform = target.GetComponent<TransformComponent>();
			TSVector position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
			TSQuaternion rotation = TSQuaternion.Euler(0, param[4], 0);
			
			position = targetTransform.TransformPoint(position);
			rotation = TSQuaternion.FromToRotation(TSVector.up, targetTransform.Upwards) * rotation;
			LSUnitFactory.CreateBulletToDirection(owner.LSWorld(), param[0], param[5], position, rotation, owner);
		}
	}
}
