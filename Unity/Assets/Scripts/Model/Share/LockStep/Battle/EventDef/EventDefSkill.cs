using TrueSync;

namespace ET
{
    public enum FloatingType
    {
        Damage,
        Heal,
        Exp,
        Critical,
        Dodge,
        Miss,
        Block
    }

    public struct LSUnitCasting
    {
        public long Id;
        public int SkillId;
    }

    public struct LSUnitFloating
    {
        public long Id;
        public FP Value;
        public FloatingType Type;
    }

    public struct LSUnitFx
    {
        public long Id;
        public int FxId;
    }
}