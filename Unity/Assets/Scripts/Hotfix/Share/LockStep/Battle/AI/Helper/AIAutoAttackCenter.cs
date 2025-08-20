using NPBehave;
using TrueSync;

namespace ET
{
    public static class AIAutoAttackCenter
    {
        public static Node Gen()
        {
            return new ServiceAwareness(FP.Half, FP.EN1,
                new Selector(
                    
                    new BlackboardBool(AIConstValue.HasEnemy, Operator.IS_EQUAL, false, Stops.IMMEDIATE_RESTART,
                        new TaskFlowFieldMove()
                    ),
                    new Repeater(
                        new Sequence(
                            new ActionAttack(),
                            new WaitSecond(FP.Half)
                        )
                    )
                )
            );
        }
    }
}