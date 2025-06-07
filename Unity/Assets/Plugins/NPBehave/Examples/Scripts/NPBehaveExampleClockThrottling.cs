﻿using UnityEngine;
using NPBehave;

/// <summary>
/// This example shows how you can use use clock instances to have complete control over how your tree receives updates.
/// This allows you for example to throttle updates to AI instances that are far away from the player.
/// You can also share clock instances by multiple trees if you like.
/// </summary>
public class NPBehaveExampleClockThrottling : MonoBehaviour
{
    // tweak this value to control how often your tree is ticked
    public float updateFrequency = 1.0f; // 1.0f = every second

    private BehaveWorld behaveWorld;
    private Root behaviorTree;
    private float accumulator = 0.0f;

    private class UpdateService : Service
    {
        public UpdateService(Node decoratee) : base(decoratee)
        {
        }

        protected override void OnService()
        {
            Debug.Log("Test");
        }
    }

    void Start()
    {
        Node mainTree = new UpdateService(new WaitUntilStopped());
        
        behaveWorld = new BehaveWorld();
        behaviorTree = new Root(behaveWorld, behaveWorld.CreateBlackboard(), mainTree);
        behaviorTree.Start();
    }

    void Update()
    {
        accumulator += Time.deltaTime;
        if (accumulator > updateFrequency)
        {
            accumulator -= updateFrequency;
            behaveWorld.Update(updateFrequency);
        }
    }
}
