using UnityEngine;

namespace NPBehave.Examples.ReusableSubtrees
{
    public class EnemyB : MonoBehaviour
    {
        private Root behaviorTree;
        
        void Start()
        {
            // this enemy is only able to move
            behaviorTree = new Root(UnityContext.GetBehaveWorld(),
                new Sequence(

                    // create movement behavior from by using our common node factory
                    NodeFactory.CreateMoveSubtree("EnemyB"),

                    // also add some custom behavior
                    new ActionLog("EnemyB attacking!")
                )
            );
            behaviorTree.Start();
        }
    }
}
