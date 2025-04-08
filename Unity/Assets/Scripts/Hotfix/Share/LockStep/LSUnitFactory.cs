using TrueSync;

namespace ET
{
    public static partial class LSUnitFactory
    {
        public static LSUnit CreateHero(LSWorld lsWorld, int id, TSVector position, TSQuaternion rotation, long playerId)
        {
	        TbHeroRow row = TbHero.Instance.Get(id);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(playerId);
			
	        lsUnit.Position = position;
	        lsUnit.Rotation = rotation;

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Hero);
	        lsUnit.AddComponent<TeamComponent, TeamType>(TeamType.TeamA);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, prop.Value);
	        }
			
			lsUnit.AddComponent<DeathComponent, bool>(false);
			lsUnit.AddComponent<BuffComponent>();
			lsUnit.AddComponent<BeHitComponent>();
			lsUnit.AddComponent<SkillComponent, int[]>(row.Skills);
			
			lsUnit.AddComponent<LSInputComponent>();
			EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
            return lsUnit;
        }
        
        public static LSUnit CreateSoldier(LSWorld lsWorld, int id, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        TbSoldierRow row = TbSoldier.Instance.Get(id, 1);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
			
	        lsUnit.Position = position;
	        lsUnit.Rotation = rotation;

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Soldier);
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, prop.Value);
	        }

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
    }
}
