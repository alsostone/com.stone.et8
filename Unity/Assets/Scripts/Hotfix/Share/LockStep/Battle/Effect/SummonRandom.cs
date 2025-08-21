using System;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [EffectExecutor(ESkillEffectType.SummonRandom)]
    [FriendOf(typeof(TeamComponent))]
    public class SummonRandom : IEffectExecutor
    {
        public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
        {
            TeamType team = target.GetComponent<TeamComponent>().Type;
            var targetTransform = target.GetComponent<TransformComponent>();
            var position = TSVector.zero;
            if (param.Length >= 4) {
                position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
            }else if (param.Length >= 3) {
                position = new TSVector(param[1], param[2], 0) * FP.EN4;
            }else if (param.Length >= 2) {
                position = new TSVector(param[1], 0, 0) * FP.EN4;
            }
            position = position.Rotation(targetTransform.Rotation.eulerAngles.y);
            
            // 通过 随机包/随机集 获得要召唤的单位
            var results = ObjectPool.Instance.Fetch<List<Tuple<EUnitType, int, int>>>();
            RandomDropHelper.Random(target.GetRandom(), param[0], results);
            foreach (var tuple in results)
            {
                for (var i = 0; i < tuple.Item3; i++)
                {
                    FP angle = targetTransform.Rotation.eulerAngles.y;
                    LSUnitFactory.SummonUnit(target.LSWorld(), tuple.Item1, tuple.Item2, targetTransform.Position + position, angle.AsInt(), team);
                }
            }
            results.Clear();
            ObjectPool.Instance.Recycle(results);
        }
    }
}
