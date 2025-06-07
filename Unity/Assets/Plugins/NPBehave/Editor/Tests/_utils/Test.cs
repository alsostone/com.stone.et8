namespace NPBehave
{
    public class Test
    {
        protected BehaveWorld BehaveWorld;
        protected TestRoot Root;
        protected Blackboard Blackboard;

        protected TestRoot CreateBehaviorTree(Node sut)
        {
            this.BehaveWorld = new BehaveWorld();
            this.Blackboard = BehaveWorld.CreateBlackboard();
            this.Root = new TestRoot(BehaveWorld, Blackboard, sut);
            return Root;
        }
    }
}