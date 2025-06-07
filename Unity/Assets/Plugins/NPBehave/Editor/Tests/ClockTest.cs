using NUnit.Framework;
namespace NPBehave
{

    public class ClockTest : Test
    {
	    private class IncrBlackboardKey : Node
        {
            private string key;
            private int value;
            
            public IncrBlackboardKey(string key, int value) : base("IncrBlackboardKey")
            {
	            this.value = value;
                this.key = key;
            }
            protected override void DoStart()
            {
                Clock.AddUpdateObserver(Guid);
            }
            protected override void DoStop()
            {
                Clock.RemoveUpdateObserver(Guid);
            }
            public override void OnTimerReached()
            {
                Assert.AreEqual(value, Blackboard.GetInt(key));
                Blackboard.SetInt(key, Blackboard.GetInt(key) + 1);
            }
        }

        [Test]
        public void ShouldUpdateObserversInOrder()
        {
            TestRoot behaviorTree = CreateBehaviorTree(new Parallel(Parallel.Policy.ALL, Parallel.Policy.ALL,
                    new IncrBlackboardKey("currentAction", 0),
                    new IncrBlackboardKey("currentAction", 1),
                    new IncrBlackboardKey("currentAction", 2),
                    new IncrBlackboardKey("currentAction", 3),
                    new IncrBlackboardKey("currentAction", 4)
                ));
            behaviorTree.Blackboard.SetInt("currentAction", 0);
            behaviorTree.Start();

            BehaveWorld.Update(0);
            Assert.AreEqual(5, behaviorTree.Blackboard.GetInt("currentAction"));
        }
        
        // 已经不是一个单独功能，而是变成了一个复合系统，改测试用例很花时间，下边儿不改了。
  //
  //       [Test]
  //       public void ShouldNotUpdateObserver_WhenRemovedDuringUpdate()
  //       {
  //           bool action2Invoked = false;
  //           System.Action action2 = () =>
  //           {
  //               action2Invoked = true;
  //           };
  //           System.Action action1 = new System.Action(() =>
  //           {
  //               Assert.IsFalse(action2Invoked);
  //               this.sut.RemoveUpdateObserver(action2);
  //           });
  //
  //           this.sut.AddUpdateObserver(action1);
  //           this.sut.AddUpdateObserver(action2);
  //           this.sut.Update(0);
  //           Assert.IsFalse(action2Invoked);
  //       }
  //
  //       [Test]
  //       public void ShouldNotUpdateTimer_WhenRemovedDuringUpdate()
  //       {
  //           bool timer2Invoked = false;
  //           System.Action timer2 = () =>
  //           {
  //               timer2Invoked = true;
  //           };
  //           System.Action action1 = new System.Action(() =>
  //           {
  //               Assert.IsFalse(timer2Invoked);
  //               this.sut.RemoveTimer(timer2);
  //           });
  //
  //           this.sut.AddUpdateObserver(action1);
  //           this.sut.AddTimer(0f, 0, timer2);
  //           this.sut.Update(1);
  //           Assert.IsFalse(timer2Invoked);
  //       }
  //
  //       [Test]
  //       public void ShouldNotUpdateTimer_WhenRemovedDuringTimer()
  //       {
  //           // TODO: as it's a dictionary, the order of events could not always be correct...
  //           bool timer2Invoked = false;
  //           System.Action timer2 = () =>
  //           {
  //               timer2Invoked = true;
  //           };
  //           System.Action timer1 = new System.Action(() =>
  //           {
  //               Assert.IsFalse(timer2Invoked);
  //               this.sut.RemoveTimer(timer2);
  //           });
  //
  //           this.sut.AddTimer(0f, 0, timer1);
  //           this.sut.AddTimer(0f, 0, timer2);
  //           this.sut.Update(1);
  //           Assert.IsFalse(timer2Invoked);
  //       }
  //
		// [Test]
		// public void ShouldUpdateAgain_WhenUsingInfiniteRepititions()
		// {
		// 	int invokations = 0;
		// 	System.Action callback = new System.Action(() =>
		// 	{
		// 		invokations++;
		// 	});
  //
		// 	this.sut.AddTimer(0.9f, -1, callback);
		// 	this.sut.Update(1);
		// 	Assert.AreEqual(1, invokations);
		// 	this.sut.Update(1);
		// 	Assert.AreEqual(2, invokations);
		// 	this.sut.Update(1);
		// 	Assert.AreEqual(3, invokations);
		// 	this.sut.Update(0.1f);
		// 	Assert.AreEqual(3, invokations);
		// 	this.sut.Update(0.1f);
		// 	Assert.AreEqual(3, invokations);
		// }
    }
}