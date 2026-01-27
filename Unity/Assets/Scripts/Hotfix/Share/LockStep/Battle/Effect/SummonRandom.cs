using System;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [EffectExecutor(EffectActionType.SummonRandom)]
    [FriendOf(typeof(TeamComponent))]
    public class SummonRandom : IEffectExecutor
    {
        public void Run(int[] param, int count, LSUnit owner, LSUnit target, LSUnit carrier = null)
        {
            var targetTransform = target.GetComponent<TransformComponent>();
            var position = TSVector.zero;
            if (param.Length >= 4) {
                position = new TSVector(param[1], param[2], param[3]) * FP.EN4;
            } else if (param.Length >= 3) {
                position = new TSVector(param[1], param[2], 0) * FP.EN4;
            } else if (param.Length >= 2) {
                position = new TSVector(param[1], 0, 0) * FP.EN4;
            }
            FP angle = targetTransform.Rotation.eulerAngles.y;
            position = position.Rotation(angle);
            
            // 通过 随机包/随机集 获得要召唤的单位
            TeamComponent teamComponent = owner.GetComponent<TeamComponent>();
            LSUnit lsOwnerOwner = target.LSUnit(teamComponent.OwnerId);
            var results = ObjectPool.Instance.Fetch<List<LSRandomDropItem>>();
            RandomDropHelper.Random(target.GetRandom(), param[0], results);
            foreach (var item in results)
            {
                for (var i = 0; i < item.Count; i++)
                {
                    LSUnitFactory.SummonUnit(lsOwnerOwner, item.Type, item.TableId, targetTransform.Position + position, angle.AsInt(), teamComponent.Type);
                }
            }
            results.Clear();
            ObjectPool.Instance.Recycle(results);
        }
    }
}
