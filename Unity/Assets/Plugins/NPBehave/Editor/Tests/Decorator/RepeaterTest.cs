using NUnit.Framework;
using TrueSync;

namespace NPBehave
{
    public class RepeaterTest : Test
    {
        [Test]
        public void ShouldFail_WhenDecorateeFails()
        {
            MockNode failingChild = new MockNode();
            Repeater sut = new Repeater(-1, failingChild);
            TestRoot behaviorTree = CreateBehaviorTree(sut);

            behaviorTree.Start();
            Assert.AreEqual(Node.State.ACTIVE, sut.CurrentState);

            failingChild.Finish(false);

            Assert.AreEqual(Node.State.INACTIVE, sut.CurrentState);
            Assert.IsTrue(behaviorTree.DidFinish);
            Assert.IsFalse(behaviorTree.WasSuccess);
        }

        [Test]
        public void ShouldSucceed_WhenDecorateeSucceededGivenTimes()
        {
            MockNode succeedingChild = new MockNode();
            Repeater sut = new Repeater(3, succeedingChild);
            TestRoot behaviorTree = CreateBehaviorTree(sut);

            behaviorTree.Start();
            Assert.AreEqual(Node.State.ACTIVE, sut.CurrentState);

            for (int i = 0; i < 2; i++)
            {
                succeedingChild.Finish(true);
                Assert.AreEqual(Node.State.ACTIVE, sut.CurrentState);
                Assert.IsFalse(behaviorTree.DidFinish);
                BehaveWorld.Update(FP.Ratio(1, 100));
            }

            succeedingChild.Finish(true);
            Assert.AreEqual(Node.State.INACTIVE, sut.CurrentState);
            Assert.IsTrue(behaviorTree.DidFinish);
            Assert.IsTrue(behaviorTree.WasSuccess);
        }

        [Test]
        public void ShouldWaitForNextUpdate_WhenDecorateeSucceedsImmediately()
        {
            MockNode succeedingChild = new MockNode();
            Repeater sut = new Repeater(-1, succeedingChild);
            TestRoot behaviorTree = CreateBehaviorTree(sut);

            behaviorTree.Start();
            Assert.AreEqual(Node.State.ACTIVE, sut.CurrentState);
            succeedingChild.Finish(true);

            // the child should not have been immediately restarted
            Assert.AreEqual(Node.State.INACTIVE, succeedingChild.CurrentState);

            // after update it's ok to have
            BehaveWorld.Update(FP.Ratio(1, 100));
            Assert.AreEqual(Node.State.ACTIVE, succeedingChild.CurrentState);

            Assert.IsFalse(behaviorTree.DidFinish);
        }


        [Test]
        public void ShouldNotLeaveObserversRegistered_WhenInactive()
        {
            MockNode child = new MockNode();
            Repeater sut = new Repeater(-1, child);
            TestRoot behaviorTree = CreateBehaviorTree(sut);

            behaviorTree.Start();
            Assert.AreEqual(Node.State.ACTIVE, sut.CurrentState);

            child.Finish(true);

            Assert.AreEqual(1, behaviorTree.Clock.NumTimers);
            Assert.AreEqual(0, behaviorTree.Clock.NumUpdateObservers);

            BehaveWorld.Update(FP.Ratio(1, 100));
            child.Finish(false);

            Assert.IsTrue(behaviorTree.DidFinish);
            Assert.IsFalse(behaviorTree.WasSuccess);

            Assert.AreEqual(0, behaviorTree.Clock.NumTimers);
            Assert.AreEqual(0, behaviorTree.Clock.NumUpdateObservers);
        }
    }
}