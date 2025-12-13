namespace ET
{
	public interface IEffectExecutor
	{
		void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null);
	}
}
