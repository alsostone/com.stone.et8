using NPBehave;
using TrueSync;

namespace ET
{
    public static class AIAutoAttack
    {
        public static Node Gen()
        {
            return new ServiceAwareness(FP.Half, FP.EN1,
                new Selector(
                    
                    new BlackboardBool(AIConstValue.HasEnemy, Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART, 
                        new Repeater(
                            new Selector(
                                new RequestAttack(),
                                new WaitSecond(FP.Half)
                            )
                        )
                    ),
                    new WaitUntilStopped()
                )
            );
        }
    }
}