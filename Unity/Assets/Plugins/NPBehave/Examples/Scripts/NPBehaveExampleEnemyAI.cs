using UnityEngine;
using NPBehave;
using NPBehave.Examples;

public class NPBehaveExampleEnemyAI : MonoBehaviour
{
    private Root behaviorTree;

    void Start()
    {
        // create our behaviour tree and get it's blackboard
        behaviorTree = CreateBehaviourTree();

        // attach the debugger component if executed in editor (helps to debug in the inspector) 
#if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = behaviorTree;
#endif

        // start the behaviour tree
        behaviorTree.Start();
    }
    
    private class UpdateService : Service
    {
        private readonly Transform transform;
        
        public UpdateService(Transform transform, float interval, Node decoratee) : base(interval, decoratee)
        {
            this.transform = transform;
        }
        
        protected override void OnService()
        {
            Vector3 playerLocalPos = transform.InverseTransformPoint(GameObject.FindGameObjectWithTag("Player").transform.position);
            Blackboard.SetFloat("playerLocalPosX", playerLocalPos.x);
            Blackboard.SetFloat("playerLocalPosY", playerLocalPos.y);
            Blackboard.SetFloat("playerLocalPosZ", playerLocalPos.z);
            Blackboard.SetFloat("playerDistance", playerLocalPos.magnitude);
        }
    }
    
    private Root CreateBehaviourTree()
    {
        // we always need a root node
        var transform1 = transform;
        return new Root(UnityContext.GetBehaveWorld(),

            // kick up our service to update the "playerDistance" and "playerLocalPos" Blackboard values every 125 milliseconds
            new UpdateService(transform1, 0.125f,

                new Selector(

                    // check the 'playerDistance' blackboard value.
                    // When the condition changes, we want to immediately jump in or out of this path, thus we use IMMEDIATE_RESTART
                    new BlackboardFloat("playerDistance", Operator.IS_SMALLER, 7.5f, Stops.IMMEDIATE_RESTART,

                        // the player is in our range of 7.5f
                        new Sequence(

                            // set color to 'red'
                            new ActionColor(transform1, Color.red) { Label = "Change to Red" },

                            // go towards player until playerDistance is greater than 7.5 ( in that case, _shouldCancel will get true )
                            new ActionTowards(transform1) { Label = "Follow" }
                        )
                    ),

                    // park until playerDistance does change
                    new Sequence(
                        new ActionColor(transform1, Color.grey) { Label = "Change to Gray" },
                        new WaitUntilStopped()
                    )
                )
            )
        );
    }
    
}
