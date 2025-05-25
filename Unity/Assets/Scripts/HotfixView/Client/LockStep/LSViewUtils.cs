namespace ET.Client
{
    public static class LSViewUtils
    {
        public static LSUnitView LSViewOwner(this Entity entity)
        {
            if (entity.Parent is LSUnitView lsUnit)
                return lsUnit;
            return entity.Parent.Parent as LSUnitView;
        }

    }
}
