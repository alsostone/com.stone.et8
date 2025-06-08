using NUnit.Framework;
using TrueSync;

namespace NPBehave
{
    public class BlackboardQueryTest : Test
    {
        private class BlackboardQuery2 : BlackboardQuery
        {
            public BlackboardQuery2(string[] blackboardKeys, Stops stopsOnChange, Node decoratee) : base(blackboardKeys, stopsOnChange, decoratee)
            {
            }
            protected override bool IsConditionMet()
            {
                var f1 = Blackboard.GetFloat("key1");
                var f2 = Blackboard.GetFloat("key2");

                if ((f1 > FP.Ratio(99, 100)) && (f2 < FP.Ratio(599, 100)))
                    return true;
                return false;
            }
        }
        
        [Test]
        public void ShouldNotThrowErrors_WhenObservingKeysThatDontExist()
        {
            TestRoot behaviorTree = null;
            MockNode child = new MockNode();
            BlackboardQuery2 sut = new BlackboardQuery2(new string[]{"key1", "key2"}, Stops.IMMEDIATE_RESTART, child);
            behaviorTree = CreateBehaviorTree(sut);

            behaviorTree.Start();
            Assert.AreEqual(Node.State.INACTIVE, sut.CurrentState);
            Assert.AreEqual(Node.State.INACTIVE, child.CurrentState);
        }
    }
}