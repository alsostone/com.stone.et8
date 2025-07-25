using UnityEngine;

namespace NPBehave.Examples
{
    public class NavMoveTo : Task
    {
        private const float DESTINATION_CHANGE_THRESHOLD = 0.0001f;
        private const uint DESTINATION_CHANGE_MAX_CHECKS = 100;

#if UNITY_5_3 || UNITY_5_4
        private NavMeshAgent agent;
#else
        private UnityEngine.AI.NavMeshAgent agent;
#endif
        private string blackboardKey;
        private float tolerance;
        private bool stopOnTolerance;
        private float updateFrequency;
        private float updateVariance;

        private Vector3 lastDestination;
        private float lastDistance;
        private uint failedChecks;

        /// CAUTION: EXPERIMENTAL !!!!
        /// <param name="agent">target to move</param>
        /// <param name="blackboardKey">blackboard key containing either a Transform or a Vector.</param>
        /// <param name="tolerance">acceptable tolerance</param>
        /// <param name="stopOnTolerance">should stop when in tolerance</param>
        /// <param name="updateFrequency">frequency to check for changes of reaching the destination or a Transform's location</param>
        /// <param name="updateVariance">random variance for updateFrequency</param>

#if UNITY_5_3 || UNITY_5_4
        public NavMoveTo(NavMeshAgent agent, string blackboardKey, float tolerance = 1.0f, bool stopOnTolerance = false, float updateFrequency = 0.1f, float updateVariance = 0.025f) : base("NavMoveTo")
#else
        public NavMoveTo(UnityEngine.AI.NavMeshAgent agent, string blackboardKey, float tolerance = 1.0f, bool stopOnTolerance = false, float updateFrequency = 0.1f, float updateVariance = 0.025f) : base("NavMoveTo")
#endif
        {
            this.agent = agent;
            this.blackboardKey = blackboardKey;
            this.tolerance = tolerance;
            this.stopOnTolerance = stopOnTolerance;
            this.updateFrequency = updateFrequency;
            this.updateVariance = updateVariance;
        }

        protected override void DoStart()
        {
            lastDestination = Vector3.zero;
            lastDistance = 99999999.0f;
            failedChecks = 0;

            Blackboard.AddObserver(blackboardKey, Guid);
            Clock.AddTimer(updateFrequency, updateVariance, -1, Guid);

            moveToBlackboardKey();
        }

        protected override void DoStop()
        {
            stopAndCleanUp(false);
        }

        private void OnValueChanged(BlackboardChangeType type, object newValue)
        {
            moveToBlackboardKey();
        }

        public override void OnTimerReached()
        {
            moveToBlackboardKey();
        }

        private void moveToBlackboardKey()
        {
            var x = Blackboard.GetFloat(blackboardKey + "X").AsFloat();
            var y = Blackboard.GetFloat(blackboardKey + "Y").AsFloat();
            var z = Blackboard.GetFloat(blackboardKey + "Z").AsFloat();
            
            // get target location
            var destination = new Vector3(x, y, z);

            // set new destination
            agent.destination = destination;

            bool destinationChanged = (agent.destination - lastDestination).sqrMagnitude > (DESTINATION_CHANGE_THRESHOLD * DESTINATION_CHANGE_THRESHOLD); //(destination - agent.destination).sqrMagnitude > (DESTINATION_CHANGE_THRESHOLD * DESTINATION_CHANGE_THRESHOLD);
            bool distanceChanged = Mathf.Abs(agent.remainingDistance - lastDistance) > DESTINATION_CHANGE_THRESHOLD;

            // check if we are already at our goal and stop the task
            if (lastDistance < this.tolerance)
            {
                if (stopOnTolerance || (!destinationChanged && !distanceChanged))
                {
                    // reached the goal
                    stopAndCleanUp(true);
                    return;
                }
            }
            else if (!destinationChanged && !distanceChanged)
            {
                if (failedChecks++ > DESTINATION_CHANGE_MAX_CHECKS)
                {
                    // could not reach the goal for whatever reason
                    stopAndCleanUp(false);
                    return;
                }
            }
            else
            {
                failedChecks = 0;
            }

            lastDestination = agent.destination;

            // Workaround for lastDistance set to 0 https://github.com/meniku/NPBehave/issues/33
            if (agent.pathPending)
            {
                lastDistance = 99999999.0f;
            }
            else
            {
                lastDistance = agent.remainingDistance;
            }
        }

        private void stopAndCleanUp(bool result)
        {
            agent.destination = agent.transform.position;
            Blackboard.RemoveObserver(blackboardKey, Guid);
            Clock.RemoveTimer(Guid);
            Stopped(result);
        }
    }
}