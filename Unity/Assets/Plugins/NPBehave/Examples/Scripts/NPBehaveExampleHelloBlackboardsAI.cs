﻿using UnityEngine;
using NPBehave;
using NPBehave.Examples;

public class NPBehaveExampleHelloBlackboardsAI : MonoBehaviour
{
    private Root behaviorTree;

    private class UpdateService : Service
    {
        public UpdateService(float interval, Node decoratee) : base(interval, decoratee)
        {
        }
        
        protected override void OnService()
        {
            Blackboard.SetBool("foo", !Blackboard.GetBool("foo"));
        }
    }

    void Start()
    {
        behaviorTree = new Root(UnityContext.GetBehaveWorld(),

            // toggle the 'toggled' blackboard boolean flag around every 500 milliseconds
            new UpdateService(0.5f,

                new Selector(

                    // Check the 'toggled' flag. Stops.IMMEDIATE_RESTART means that the Blackboard will be observed for changes 
                    // while this or any lower priority branches are executed. If the value changes, the corresponding branch will be
                    // stopped and it will be immediately jump to the branch that now matches the condition.
                    new BlackboardBool("foo", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,

                        // when 'toggled' is true, this branch will get executed.
                        new Sequence(

                            // print out a message ...
                            new ActionLog("foo"),

                            // ... and stay here until the `BlackboardValue`-node stops us because the toggled flag went false.
                            new WaitUntilStopped()
                        )
                    ),

                    // when 'toggled' is false, we'll eventually land here
                    new Sequence(
                        new ActionLog("bar"),
                        new WaitUntilStopped()
                    )
                )
            )
        );
        behaviorTree.Start();

        // attach the debugger component if executed in editor (helps to debug in the inspector) 
#if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = behaviorTree;
#endif
    }
}
