using UnityEngine;

namespace NPBehave.Examples
{
    public class ActionTowards : ActionRequest
    {
        private readonly Transform transform;

        public ActionTowards(Transform transform)
        {
            this.transform = transform;
        }
        
        protected override Result OnAction(Request request)
        {
            switch (request)
            {
                case Request.START:
                    return Result.PROGRESS;
                case Request.UPDATE:
                {
                    var x = Blackboard.GetFloat("playerLocalPosX").AsFloat();
                    var y = Blackboard.GetFloat("playerLocalPosY").AsFloat();
                    var z = Blackboard.GetFloat("playerLocalPosZ").AsFloat();
                    Vector3 pos = new Vector3(x, y, z);
                    transform.localPosition += pos * (0.5f * Time.deltaTime);
                    return Result.PROGRESS;
                }
                case Request.CANCEL:
                    return Result.SUCCESS;
            }
            return Result.FAILED;
        }
    }
}