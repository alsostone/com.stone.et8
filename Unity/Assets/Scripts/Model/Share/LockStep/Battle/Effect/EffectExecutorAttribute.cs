namespace ET
{
    public class EffectExecutorAttribute: BaseAttribute
    {
        public EffectActionType ActionType { get; }

        public EffectExecutorAttribute(EffectActionType type)
        {
            this.ActionType = type;
        }
    }
}