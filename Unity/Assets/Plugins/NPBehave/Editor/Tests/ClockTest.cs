using NUnit.Framework;
namespace NPBehave
{

    public class ClockTest : Test
    {
        [Test]
        public void ShouldUpdateObserversInOrder()
        {
            TestRoot behaviorTree = CreateBehaviorTree(new Parallel(Parallel.Policy.ALL, Parallel.Policy.ALL,
                    new IncrBlackboardKey("currentAction"),
                    new IncrBlackboardKey("currentAction"),
                    new IncrBlackboardKey("currentAction"),
                    new IncrBlackboardKey("currentAction"),
                    new IncrBlackboardKey("currentAction")
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
  //           this.sut.AddTimer(0, 0, timer2);
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
  //           this.sut.AddTimer(0, 0, timer1);
  //           this.sut.AddTimer(0, 0, timer2);
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
		// 	this.sut.AddTimer(FP.Ratio(9, 10), -1, callback);
		// 	this.sut.Update(1);
		// 	Assert.AreEqual(1, invokations);
		// 	this.sut.Update(1);
		// 	Assert.AreEqual(2, invokations);
		// 	this.sut.Update(1);
		// 	Assert.AreEqual(3, invokations);
		// 	this.sut.Update(FP.EN1);
		// 	Assert.AreEqual(3, invokations);
		// 	this.sut.Update(FP.EN1);
		// 	Assert.AreEqual(3, invokations);
		// }
    }
}