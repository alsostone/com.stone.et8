namespace ET
{
	public interface IEffectExecutor
	{
		void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null);
	}
}
