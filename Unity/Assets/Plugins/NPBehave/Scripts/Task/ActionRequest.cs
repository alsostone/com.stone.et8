
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace NPBehave
{
    public enum Result
    {
        SUCCESS,
        FAILED,
        BLOCKED,
        PROGRESS
    }

    public enum Request
    {
        START,
        UPDATE,
        CANCEL,
    }

    public abstract class ActionRequest : Task
    {
        [BsonElement][MemoryPackInclude] private bool bWasBlocked = false;

        protected ActionRequest() : base("ActionRequest")
        {
        }

        protected override void DoStart()
        {
            Result result = OnAction(Request.START);
            if (result == Result.PROGRESS)
            {
                Clock.AddUpdateObserver(Guid);
            }
            else if (result == Result.BLOCKED)
            {
                bWasBlocked = true;
                Clock.AddUpdateObserver(Guid);
            }
            else
            {
                Stopped(result == Result.SUCCESS);
            }
        }
        
        public override void OnTimerReached()
        {
            Result result = OnAction(bWasBlocked ? Request.START : Request.UPDATE);
            if (result == Result.BLOCKED)
            {
                bWasBlocked = true;
            }
            else if (result == Result.PROGRESS)
            {
                bWasBlocked = false;
            }
            else
            {
                Clock.RemoveUpdateObserver(Guid);
                Stopped(result == Result.SUCCESS);
            }
        }

        protected override void DoStop()
        {
            Result result = OnAction(Request.CANCEL);
            Clock.RemoveUpdateObserver(Guid);
            Stopped(result == Result.SUCCESS);
        }
        
        protected abstract Result OnAction(Request request);
    }
}