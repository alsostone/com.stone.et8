using TrueSync;

namespace ET
{
    [EffectExecutor(EffectActionType.SummonSoldier)]
    [FriendOf(typeof(TeamComponent))]
    public class SummonSoldier : IEffectExecutor
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
            
            TeamComponent teamComponent = owner.GetComponent<TeamComponent>();
            LSUnit lsOwnerOwner = target.LSUnit(teamComponent.OwnerId);
            LSUnitFactory.CreateSoldier(lsOwnerOwner, param[0], targetTransform.Position + position, angle.AsInt(), teamComponent.Type);
        }
    }
}
