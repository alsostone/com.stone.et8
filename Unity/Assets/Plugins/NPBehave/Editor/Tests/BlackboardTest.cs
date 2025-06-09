using NUnit.Framework;
using TrueSync;

namespace NPBehave
{
#pragma warning disable 618 // deprecation

    public class BlackboardTest : Test
    {
        private class SetBlackboardKey : Node
        {
            private string observerName;
            private string key;
            private bool value;
            
            public SetBlackboardKey(string observerName, string key, bool value) : base("SetBlackboardKey")
            {
                this.observerName = observerName;
                this.key = key;
                this.value = value;
            }
            protected override void DoStart()
            {
                Blackboard.AddObserver(this.observerName, Guid);
            }
            protected override void DoStop()
            {
                Blackboard.RemoveObserver(this.observerName, Guid);
            }
            public override void OnObservingChanged(BlackboardChangeType type)
            {
                Blackboard.SetBool(key, value);
            }
        }
        
        [Test]
        public void ShouldNotNotifyObservers_WhenNoClockUpdate()
        {
            var setKey = new SetBlackboardKey("test", "notified", true);
            TestRoot behaviorTree = CreateBehaviorTree(setKey);
            
            Blackboard.SetBool("notified", false);
            behaviorTree.Start();

            Blackboard.SetFloat("test", FP.One);
            Assert.IsFalse(behaviorTree.Blackboard.GetBool("notified"));
        }

        [Test]
        public void ShouldNotifyObservers_WhenClockUpdate()
        {
            var setKey = new SetBlackboardKey("test", "notified", true);
            TestRoot behaviorTree = CreateBehaviorTree(setKey);
            
            Blackboard.SetBool("notified", false);
            behaviorTree.Start();
            
            Blackboard.SetFloat("test", FP.One);
            BehaveWorld.Update(FP.One);
            Assert.IsTrue(Blackboard.GetBool("notified"));
        }

        [Test]
        public void ShouldAllowToSetToNull_WhenAlreadySertToNull()
        {
            var behaveWorld = new BehaveWorld();
            var blackboard = behaveWorld.CreateBlackboard();
            blackboard.SetFloat("test", FP.One);
            Assert.AreEqual(blackboard.GetFloat("test"), FP.One);
            blackboard.UnSetFloat("test");
            Assert.AreEqual(blackboard.GetFloat("test"), FP.Zero);
            blackboard.SetBool("test", true);
            Assert.AreEqual(blackboard.GetBool("test"), true);
        }

        // check for https://github.com/meniku/NPBehave/issues/17
        [Test]
        public void ShouldListenToEvents_WhenUsingChildBlackboard()
        {
            var behaveWorld = new BehaveWorld();

            Blackboard rootBlackboard = behaveWorld.CreateBlackboard();
            Blackboard blackboard = behaveWorld.CreateBlackboard(rootBlackboard);

            // our mock nodes we want to query for status
            MockNode firstChild = new MockNode(false); // false -> fail when aborted
            MockNode secondChild = new MockNode(false);

            // conditions for each subtree that listen the BB for events
            var firstCondition = new BlackboardBool("branch1", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART, firstChild);
            var secondCondition = new BlackboardBool("branch2", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART, secondChild);

            // set up the tree
            Selector selector = new Selector(firstCondition, secondCondition);
            TestRoot behaviorTree = new TestRoot(behaveWorld, blackboard, selector);

            // intially we want to activate branch2
            rootBlackboard.SetBool("branch2", true);

            // start the tree
            behaviorTree.Start();

            // tick the timer to ensure the blackboard notifies the nodes
            behaveWorld.Update(FP.EN1);

            // verify the second child is running
            Assert.AreEqual(Node.State.INACTIVE, firstChild.CurrentState);
            Assert.AreEqual(Node.State.ACTIVE, secondChild.CurrentState);

            // change keys so the first conditions get true, too
            rootBlackboard.SetBool("branch1", true);

            // tick the timer to ensure the blackboard notifies the nodes
            behaveWorld.Update(FP.EN1);

            // now we should be in branch1
            Assert.AreEqual(Node.State.ACTIVE, firstChild.CurrentState);
            Assert.AreEqual(Node.State.INACTIVE, secondChild.CurrentState);
        }
    }
}