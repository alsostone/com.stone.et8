using UnityEngine;
using NPBehave;
using NPBehave.Examples;

public class NPBehaveExampleHelloWorldAI : MonoBehaviour
{
    private Root behaviorTree;

    void Start()
    {
        behaviorTree = new Root(UnityContext.GetBehaveWorld(),
            new ActionLog("Hello World!")
        );
        behaviorTree.Start();
    }
}
