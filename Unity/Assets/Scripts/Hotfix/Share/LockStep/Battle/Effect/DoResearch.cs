using System.Collections.Generic;

namespace ET
{
	[EffectExecutor(EffectActionType.Research)]
	public class DoResearch : IEffectExecutor
	{
		public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
		{
			if (param.Length != 2) { return; }
            
			var distances = ObjectPool.Instance.Fetch<List<SearchUnit>>();
			TargetSearcher.Search(param[0], carrier, distances);
			EffectExecutor.Execute(param[1], owner, distances, count);
			distances.Clear();
			ObjectPool.Instance.Recycle(distances);
		}
	}
}
