using UnityEngine;
using NPBehave;

namespace NPBehave.Examples.ReusableSubtrees
{
    public class NodeFactory
    {
        public static Node CreateMoveSubtree(string enemyName)
        {
            // shared movement behavior, you can create any trees of any depth
            return new Cooldown(3.0f, 
                new ActionLog(enemyName + " moving!")
            );
        }
    }
}
