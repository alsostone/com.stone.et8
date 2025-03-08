namespace ET
{
    [EntitySystemOf(typeof(Unit))]
    public static partial class UnitSystem
    {
        [EntitySystem]
        private static void Awake(this Unit self, int configId)
        {
            self.ConfigId = configId;
        }

        public static TbUnitRow Config(this Unit self)
        {
            return TbUnit.Instance.Get(self.ConfigId);
        }
        
        public static UnitType Type(this Unit self)
        {
            return (UnitType)self.Config().Type;
        }
    }
}