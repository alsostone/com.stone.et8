namespace ET
{
	[EffectExecutor(EffectActionType.AddBulletDirection)]
	public class AddBulletDirection : IEffectExecutor
	{
		public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			if (param.Length < 3) { return; }

			int angle = param[1];
			int searchId = param[2];
			LSUnitFactory.CreateBullet(owner.LSWorld(), param[0], angle, searchId, owner, target);
		}
	}
}
