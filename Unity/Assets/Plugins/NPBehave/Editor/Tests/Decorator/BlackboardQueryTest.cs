using System;
using NUnit.Framework;

namespace NPBehave
{
    public class BlackboardQueryTest : Test
    {
        private class WhenObservingKeysThatDontExist : BlackboardQuery
        {
            public WhenObservingKeysThatDontExist(string[] keys, Stops stopsOnChange, Node decoratee) : base(keys, stopsOnChange, decoratee)
            {
            }
        
            protected override bool IsConditionMet()
            {
                object o1 = Blackboard.Get<float>("key1");
                object o2 = Blackboard.Get<float>("key2");
                float f1 = (float)o1;
                float f2 = (float)o2;

                if ((f1 > 0.99) && (f2 < 5.99f))
                    return true;
                return false;
            }
        }
        
        [Test]
        public void ShouldNotThrowErrors_WhenObservingKeysThatDontExist()
        {
            TestRoot behaviorTree = null;
            MockNode child = new MockNode();
            var sut = new WhenObservingKeysThatDontExist(new string[]{"key1", "key2"}, Stops.IMMEDIATE_RESTART, child);
            behaviorTree = CreateBehaviorTree(sut);

            behaviorTree.Start();
            Assert.AreEqual(Node.State.INACTIVE, sut.CurrentState);
            Assert.AreEqual(Node.State.INACTIVE, child.CurrentState);
        }
    }
}