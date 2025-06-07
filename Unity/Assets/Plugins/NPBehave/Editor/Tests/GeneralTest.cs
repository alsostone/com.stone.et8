using NUnit.Framework;
namespace NPBehave
{

    public class GeneralTest : Test
    {
        [Test]
        public void ShouldNotActivateLowerPriorityBranchInCaseMultipleBranchesGetValid()
        {
            var behaveWorld = new BehaveWorld();
            this.Blackboard = behaveWorld.CreateBlackboard();

            // our mock nodes we want to query for status
            MockNode firstChild = new MockNode(false); // false -> fail when aborted
            MockNode secondChild = new MockNode(false);
            MockNode thirdChild = new MockNode(false);

            // coniditions for each subtree that listen the BB for events
            var firstCondition = new BlackboardBool( "branch1", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART, firstChild );
            var secondCondition = new BlackboardBool( "branch2", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART, secondChild );
            var thirdCondtion = new BlackboardBool( "branch3", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART, thirdChild );

            // set up the tree
            Selector selector = new Selector(firstCondition, secondCondition, thirdCondtion);
            TestRoot behaviorTree = new TestRoot(behaveWorld, Blackboard, selector);

            // intially we want to activate branch3
            Blackboard.SetBool("branch3", true);
           
            // start the tree
            behaviorTree.Start();

            // verify the third child is running
            Assert.AreEqual(Node.State.INACTIVE, firstChild.CurrentState);
            Assert.AreEqual(Node.State.INACTIVE, secondChild.CurrentState);
            Assert.AreEqual(Node.State.ACTIVE, thirdChild.CurrentState);
            Assert.AreEqual(0, firstChild.DebugNumStartCalls);
            Assert.AreEqual(0, secondChild.DebugNumStartCalls);
            Assert.AreEqual(1, thirdChild.DebugNumStartCalls);

            // change keys so the first & second conditions get true, too
            Blackboard.SetBool("branch1", true);
            Blackboard.SetBool("branch2", true);

            // still the third child should be active, as the blackboard didn't yet notifiy the nodes
            Assert.AreEqual(Node.State.INACTIVE, firstChild.CurrentState);
            Assert.AreEqual(Node.State.INACTIVE, secondChild.CurrentState);
            Assert.AreEqual(Node.State.ACTIVE, thirdChild.CurrentState);
            Assert.AreEqual(0, firstChild.DebugNumStartCalls);
            Assert.AreEqual(0, secondChild.DebugNumStartCalls);
            Assert.AreEqual(1, thirdChild.DebugNumStartCalls);

            // tick the timer to ensure the blackboard notifies the nodes
            behaveWorld.Update(0.1f);

            // now we should be in branch1
            Assert.AreEqual(Node.State.ACTIVE, firstChild.CurrentState);
            Assert.AreEqual(Node.State.INACTIVE, secondChild.CurrentState);
            Assert.AreEqual(Node.State.INACTIVE, thirdChild.CurrentState);
            Assert.AreEqual(1, firstChild.DebugNumStartCalls);
            Assert.AreEqual(0, secondChild.DebugNumStartCalls);
            Assert.AreEqual(1, thirdChild.DebugNumStartCalls);

            // disable first branch
            Blackboard.SetBool("branch1", false);
            behaveWorld.Update(0.1f);

            // and now the second branch should be active
            Assert.AreEqual(Node.State.INACTIVE, firstChild.CurrentState);
            Assert.AreEqual(Node.State.ACTIVE, secondChild.CurrentState);
            Assert.AreEqual(Node.State.INACTIVE, thirdChild.CurrentState);
            Assert.AreEqual(1, firstChild.DebugNumStartCalls);
            Assert.AreEqual(1, secondChild.DebugNumStartCalls);
            Assert.AreEqual(1, thirdChild.DebugNumStartCalls);
        }
    }
}