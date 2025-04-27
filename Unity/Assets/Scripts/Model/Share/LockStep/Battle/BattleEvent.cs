namespace ET
{
    public struct LSUnitCreate
    {
        public LSUnit LSUnit;
    }
    
    public struct LSUnitRemove
    {
        public long Id;
    }

    public enum FloatingType
    {
        Damage,
        Heal,
        Exp,
        Dodge,
        Miss,
        Block
    }
    
    public struct LSUnitFloating
    {
        public long Id;
        public long Value;
        public FloatingType Type;
    }
}

