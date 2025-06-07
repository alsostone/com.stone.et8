using System;
using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class BlackboardInt : ObservingDecorator
    {
        [MemoryPackInclude] private readonly string blackboardKey;
        [MemoryPackInclude] private readonly int value;
        [MemoryPackInclude] private readonly Operator op;

        [MemoryPackIgnore] public string BlackboardKey => blackboardKey;
        [MemoryPackIgnore] public int Value => value;
        [MemoryPackIgnore] public Operator Operator => op;

        [MemoryPackConstructor]
        public BlackboardInt(string blackboardKey, Operator op, int value, Stops stopsOnChange, Node decoratee) : base("BlackboardInt", stopsOnChange, decoratee)
        {
            this.op = op;
            this.blackboardKey = blackboardKey;
            this.value = value;
        }
        
        public BlackboardInt(string blackboardKey, Operator op, Stops stopsOnChange, Node decoratee) : base("BlackboardInt", stopsOnChange, decoratee)
        {
            this.op = op;
            this.blackboardKey = blackboardKey;
        }
        
        protected override void StartObserving()
        {
            Blackboard.AddObserver(blackboardKey, Guid);
        }

        protected override void StopObserving()
        {
            Blackboard.RemoveObserver(blackboardKey, Guid);
        }
        
        public override void OnObservingChanged(NotifyType type)
        {
            Evaluate();
        }

        protected override bool IsConditionMet()
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!Blackboard.IsSetInt(blackboardKey))
            {
                return op == Operator.IS_NOT_SET;
            }

            var o = Blackboard.GetInt(blackboardKey);
            switch (op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL: return o.CompareTo(value) == 0;
                case Operator.IS_NOT_EQUAL: return o.CompareTo(value) != 0;
                case Operator.IS_GREATER_OR_EQUAL: return o.CompareTo(value) >= 0;
                case Operator.IS_GREATER: return o.CompareTo(value) > 0;
                case Operator.IS_SMALLER_OR_EQUAL: return o.CompareTo(value) <= 0;
                case Operator.IS_SMALLER: return o.CompareTo(value) < 0;
                default: return false;
            }
        }

        public override string ToString()
        {
            return "(" + op + ") " + blackboardKey + " ? " + value;
        }
    }
}