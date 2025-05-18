using TrueSync;

namespace ET
{
    public static partial class LSUnitFactory
    {
        public static LSUnit CreateHero(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, long playerId)
        {
	        TbHeroRow row = TbHero.Instance.Get(tableId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(playerId);
	        lsUnit.Active = true;
	        lsUnit.Position = position;
	        lsUnit.Rotation = rotation;

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Hero);
	        lsUnit.AddComponent<TeamComponent, TeamType>(TeamType.TeamA);
	        lsUnit.AddComponent<HeroComponent, int>(tableId);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, prop.Value * FP.EN4, false);
	        }
	        EnsureRuntimeProp(propComponent);
			
			lsUnit.AddComponent<DeathComponent, bool>(false);
			lsUnit.AddComponent<BuffComponent>();
			lsUnit.AddComponent<BeHitComponent>();
			lsUnit.AddComponent<SkillComponent, int[]>(row.Skills);
			
			lsUnit.AddComponent<LSInputComponent>();
			EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
            return lsUnit;
        }
        
        public static LSUnit CreateSoldier(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        TbSoldierRow row = TbSoldier.Instance.Get(tableId, 1);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.Active = true;
	        lsUnit.Position = position;
	        lsUnit.Rotation = rotation;

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Soldier);
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        lsUnit.AddComponent<SoldierComponent, int, int>(tableId, 1);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, prop.Value * FP.EN4, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        lsUnit.AddComponent<SkillComponent, int[]>(row.Skills);
			
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateBuilding(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        TbSoldierRow row = TbSoldier.Instance.Get(tableId, 1);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.Active = true;
	        lsUnit.Position = position;
	        lsUnit.Rotation = rotation;

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Building);
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        lsUnit.AddComponent<BuildingComponent, int, int>(tableId, 1);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, prop.Value * FP.EN4, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<DeathComponent, bool>(false);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        lsUnit.AddComponent<SkillComponent, int[]>(row.Skills);
			
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateBullet(LSWorld lsWorld, int bulletId, TSVector position, TSQuaternion rotation, LSUnit caster, LSUnit target)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
			
	        lsUnit.Position = position;
	        lsUnit.Rotation = rotation;

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<BulletComponent, int, LSUnit, LSUnit>(bulletId, caster, target);
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        // 确保实时属性存在
        private static void EnsureRuntimeProp(PropComponent propComponent)
		{
			if (!propComponent.Contains(NumericType.Hp)) {
				propComponent.Set(NumericType.Hp, propComponent.Get(NumericType.MaxHp));
			}
			if (!propComponent.Contains(NumericType.Mp)) {
				propComponent.Set(NumericType.Mp, propComponent.Get(NumericType.MaxMp));
			}
		}
    }
}
