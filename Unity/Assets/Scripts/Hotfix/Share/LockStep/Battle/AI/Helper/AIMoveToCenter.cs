using NPBehave;

namespace ET
{
    public static class AIMoveToCenter
    {
        public static Node Gen()
        {
            return new Sequence(
                new TaskFlowFieldMove(),
                new RequestAttack()
            );
        }
    }
}