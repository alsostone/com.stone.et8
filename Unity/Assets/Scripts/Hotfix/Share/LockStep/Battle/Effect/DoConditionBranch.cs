namespace ET
{
    [EffectExecutor(EffectActionType.ConditionBranch)]
    public class DoConditionBranch : IEffectExecutor
    {
        public void Run(int[] param, LSUnit owner, LSUnit target, LSUnit carrier = null)
        {
            if (param.Length < 2) { return; }
            
            bool isMet = ConditionChecker.CheckCondition(param[0], owner, target);
            if (isMet) {
                EffectExecutor.Execute(param[1], owner, target, carrier);
            } else if (param.Length >= 3) {
                EffectExecutor.Execute(param[2], owner, target, carrier);
            }
        }
    }
}
