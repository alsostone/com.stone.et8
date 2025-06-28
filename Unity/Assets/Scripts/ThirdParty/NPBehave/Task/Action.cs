
namespace NPBehave
{
    public abstract class Action : Task
    {
        protected Action() : base("Action")
        {
        }
        
        protected override void DoStart()
        {
            Stopped(OnAction());
        }

        protected abstract bool OnAction();
    }
}