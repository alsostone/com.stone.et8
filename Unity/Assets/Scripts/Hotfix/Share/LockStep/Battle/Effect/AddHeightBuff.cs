namespace ET
{
	[EffectExecutor(EffectActionType.AddHeightBuff)]
	public class AddHeightBuff : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			BuffComponent buffComponent = target.GetComponent<BuffComponent>();
			TransformComponent transformComponent = target.GetComponent<TransformComponent>();
			int layerCount = transformComponent.Position.y.AsInt();
			foreach (int buffId in param)
			{
				buffComponent.AddBuff(buffId, owner, layerCount);
			}
		}
	}
}
