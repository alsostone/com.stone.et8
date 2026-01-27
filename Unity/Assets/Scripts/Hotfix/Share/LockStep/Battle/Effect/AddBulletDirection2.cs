using TrueSync;

namespace ET
{
	[EffectExecutor(EffectActionType.AddBulletDirection2)]
	public class AddBulletDirection2 : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			// 子弹ID,x,y,z,Y轴发射角度,[飞行距离]
			if (param.Length < 5) { return; }
			
			TransformComponent targetTransform = target.GetComponent<TransformComponent>();
			TSVector position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
			TSQuaternion rotation = TSQuaternion.Euler(0, param[4], 0);
			FP distance = param.Length > 5 ? param[5] * FP.EN4 : 10000;
			
			position = targetTransform.TransformPoint(position);
			rotation = TSQuaternion.FromToRotation(TSVector.up, targetTransform.Upwards) * rotation;
			LSUnitFactory.CreateBulletToDirection2(owner, param[0], position, rotation, distance);
		}
	}
}
