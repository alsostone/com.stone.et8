using MemoryPack;
using NPBehave;
using TrueSync;

namespace ET
{
    [AINode]
    [MemoryPackable]
    public partial class RequestMoveToTarget : ActionRequest
    {
        protected override Result OnAction(Request request)
        {
            if (request == Request.START)
                return Result.PROGRESS;
            if (request == Request.CANCEL)
                return Result.FAILED;
            
            // 移向目标直接以目标位置为目的地
            // 动态避障由RVO系统处理
            LSUnit lsUnit = Agent as LSUnit;
            LSUnit lsTarget = lsUnit.LSUnit(Blackboard.GetLong(AIConstValue.HasEnemyEntityId));
            if (lsTarget == null)
                return Result.FAILED;
            
            PropComponent propUnit = lsUnit.GetComponent<PropComponent>();
            PropComponent propTarget = lsTarget.GetComponent<PropComponent>();
            FP range = propUnit.Get(NumericType.AtkRange);
            
            // 如果目标在攻击范围内，则不需要移动
            TransformComponent transformUnit = lsUnit.GetComponent<TransformComponent>();
            TransformComponent transformTarget = lsTarget.GetComponent<TransformComponent>();
            TSVector dir = transformTarget.Position - transformUnit.Position;
            
            FP distance = dir.magnitude - propUnit.Radius - propTarget.Radius;
            if (distance <= range * FP.Ratio(9, 10)) {
                return Result.SUCCESS;
            }
            
            transformUnit.RVOMove(new TSVector2(dir.x, dir.z));
            return Result.PROGRESS;
        }
    }
    
}