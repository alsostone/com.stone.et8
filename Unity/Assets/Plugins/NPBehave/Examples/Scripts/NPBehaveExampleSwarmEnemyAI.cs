using UnityEngine;
using NPBehave;
using NPBehave.Examples;

public class NPBehaveExampleSwarmEnemyAI : MonoBehaviour
{
    private Blackboard sharedBlackboard;
    private Blackboard ownBlackboard;
    private Root behaviorTree;

    void Start()
    {
        // get the shared blackboard for this kind of ai, this blackboard is shared by all instances
        sharedBlackboard = UnityContext.GetSharedBlackboard("example-swarm-ai");

        // create a new blackboard instance for this ai instance, parenting it to the sharedBlackboard.
        // This way we can also access shared values through the own blackboard.
        ownBlackboard = UnityContext.CreateBlackboard(sharedBlackboard);

        // create the behaviourTree
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
        private readonly Blackboard sharedBlackboard;
        
        public UpdateService(Blackboard sharedBlackboard, Transform transform, float interval, Node decoratee) : base(interval, decoratee)
        {
            this.sharedBlackboard = sharedBlackboard;
            this.transform = transform;
        }
        
        protected override void OnService()
        {
            Vector3 playerLocalPos = transform.InverseTransformPoint(GameObject.FindGameObjectWithTag("Player").transform.position);

            // update all our distances
            Blackboard.SetFloat("playerLocalPosX", playerLocalPos.x);
            Blackboard.SetFloat("playerLocalPosY", playerLocalPos.y);
            Blackboard.SetFloat("playerLocalPosZ", playerLocalPos.z);
            Blackboard.SetBool("playerInRange", playerLocalPos.magnitude < 7.5f);

            // if we are not yet engaging the player, but he is in range and there are not yet other 2 guys engaged with him
            if (Blackboard.GetBool("playerInRange") && !Blackboard.GetBool("engaged") && sharedBlackboard.GetInt("numEngagedActors") < 2)
            {
                // increment the shared 'numEngagedActors'
                sharedBlackboard.SetInt("numEngagedActors", sharedBlackboard.GetInt("numEngagedActors") + 1);

                // set this instance to 'engaged'
                Blackboard.SetBool("engaged", true);
            }

            // if we were engaging the player, but he is not in the range anymore
            if (!Blackboard.GetBool("playerInRange") && Blackboard.GetBool("engaged"))
            {
                // decrement the shared 'numEngagedActors'
                sharedBlackboard.SetInt("numEngagedActors", sharedBlackboard.GetInt("numEngagedActors") - 1);

                // set this instance to 'engaged'
                Blackboard.SetBool("engaged", false);
            }
        }
    }
    
    private Root CreateBehaviourTree()
    {
        // Tell the behaviour tree to use the provided blackboard instead of creating a new one
        var transform1 = transform;
        return new Root(UnityContext.GetBehaveWorld(), ownBlackboard,

            // Update values in the blackboards every 125 milliseconds
            new UpdateService(sharedBlackboard, transform1, 0.125f,

                new Selector(

                    // check the 'engaged' blackboard value.
                    // When the condition changes, we want to immediately jump in or out of this path, thus we use IMMEDIATE_RESTART
                    new BlackboardBool("engaged", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,

                        // we are currently engaged with the player
                        new Sequence(

                            // set color to 'red'
                            new ActionColor(transform1, Color.red) { Label = "Change to Red" },

                            // go towards player until we are not engaged anymore
                            new ActionTowards(transform1){ Label = "Follow" }
                        )
                    ),

                    // park until engaged does change
                    new Selector(

                        // this time we can also use NBtrStops.BOTH, which stops the current branch if the condition changes but will traverse the 
                        // tree further the normal way (in that case, doesn't make a difference at all). 
                        new BlackboardBool("playerInRange", Operator.IS_EQUAL, true, Stops.BOTH,

                            // player is not in range, mark 'yellow'
                            new Sequence(
                                new ActionColor(transform1, Color.yellow) { Label = "Change to Yellow" },
                                new WaitUntilStopped()
                            )
                        ),

                        // player is not in range, mark 'gray'
                        new Sequence(
                            new ActionColor(transform1, Color.grey) { Label = "Change to Gray" },
                            new WaitUntilStopped()
                        )
                    )
                )
            )
        );
    }

}
