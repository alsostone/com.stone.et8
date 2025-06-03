using System;
using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class BlackboardCondition<T> : ObservingDecorator where T : IComparable<T>
    {
        [MemoryPackInclude] private readonly string key;
        [MemoryPackInclude] private readonly T value;
        [MemoryPackInclude] private readonly Operator op;

        [MemoryPackIgnore]
        public string Key
        {
            get
            {
                return key;
            }
        }

        [MemoryPackIgnore]
        public T Value
        {
            get
            {
                return value;
            }
        }

        [MemoryPackIgnore]
        public Operator Operator
        {
            get
            {
                return op;
            }
        }
        
        [MemoryPackConstructor]
        public BlackboardCondition(string key, Operator op, T value, Stops stopsOnChange, Node decoratee) : base("BlackboardCondition", stopsOnChange, decoratee)
        {
            this.op = op;
            this.key = key;
            this.value = value;
        }
        
        public BlackboardCondition(string key, Operator op, Stops stopsOnChange, Node decoratee) : base("BlackboardCondition", stopsOnChange, decoratee)
        {
            this.op = op;
            this.key = key;
        }
        
        protected override void StartObserving()
        {
            Blackboard.AddObserver(key, onValueChanged);
        }

        protected override void StopObserving()
        {
            Blackboard.RemoveObserver(key, onValueChanged);
        }

        private void onValueChanged(Blackboard.Type type, object newValue)
        {
            Evaluate();
        }

        protected override bool IsConditionMet()
        {
            if (op == Operator.ALWAYS_TRUE)
            {
                return true;
            }

            if (!Blackboard.Isset(key))
            {
                return op == Operator.IS_NOT_SET;
            }

            T o = Blackboard.Get<T>(key);

            switch (this.op)
            {
                case Operator.IS_SET: return true;
                case Operator.IS_EQUAL: return o.CompareTo(this.value) == 0;
                case Operator.IS_NOT_EQUAL: return o.CompareTo(this.value) != 0;
                case Operator.IS_GREATER_OR_EQUAL: return o.CompareTo(this.value) >= 0;
                case Operator.IS_GREATER: return o.CompareTo(this.value) > 0;
                case Operator.IS_SMALLER_OR_EQUAL: return o.CompareTo(this.value) <= 0;
                case Operator.IS_SMALLER: return o.CompareTo(this.value) < 0;
                default: return false;
            }
        }

        public override string ToString()
        {
            return "(" + this.op + ") " + this.key + " ? " + this.value;
        }
    }
}