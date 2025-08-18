using NPBehave;
using ST.GridBuilder;
using TrueSync;

namespace ET
{
	[FriendOf(typeof(TeamComponent))]
    public static partial class LSUnitFactory
    {
	    public static LSUnit CreateGlobal(LSWorld lsWorld)
	    {
		    LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
		    LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(LSConstValue.GlobalIdOffset);
		    
		    lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Global);
		    lsUnit.AddComponent<PropComponent>();
		    return lsUnit;
	    }

	    public static LSUnit CreateTeam(LSWorld lsWorld, TeamType teamType)
	    {
		    LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
		    LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(LSConstValue.GlobalIdOffset - 1 - (int)teamType);
		    
		    lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Team);
		    lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
		    lsUnit.AddComponent<WorkQueueComponent>();
		    
		    PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
		    propComponent.Set(NumericType.MaxGoldBase, 100, false);
		    propComponent.Set(NumericType.MaxWoodBase, 100, false);
		    propComponent.Set(NumericType.MaxPopulationBase, 20, false);
		    
		    propComponent.Set(NumericType.Gold, 50, false);
		    propComponent.Set(NumericType.Wood, 50, false);
		    propComponent.Set(NumericType.Population, 10, false);
		    
		    return lsUnit;
	    }
	    
        public static LSUnit CreateHero(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, long playerId)
        {
	        TbHeroRow row = TbHero.Instance.Get(tableId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(playerId);

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Hero);
	        lsUnit.AddComponent<FlagComponent>();
	        lsUnit.AddComponent<TeamComponent, TeamType>(TeamType.TeamA);
	        lsUnit.AddComponent<HeroComponent, int>(tableId);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);
			
			lsUnit.AddComponent<DeathComponent, bool>(false);
			lsUnit.AddComponent<BuffComponent>();
			lsUnit.AddComponent<BeHitComponent>();
			lsUnit.AddComponent<SkillComponent, int[]>(row.Skills);
			
			lsUnit.AddComponent<LSGridBuilderComponent>();
			lsUnit.AddComponent<LSCommandsRunComponent>();
			
			EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
            return lsUnit;
        }
        
        public static LSUnit CreateSoldier(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        TbSoldierRow row = TbSoldier.Instance.Get(tableId, 1);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Soldier);
	        lsUnit.AddComponent<FlagComponent>();
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        lsUnit.AddComponent<SoldierComponent, int, int>(tableId, 1);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        lsUnit.AddComponent<SkillComponent, int[]>(row.Skills);
	        
	        lsUnit.AddComponent<FieldMoveComponent>();
	        lsUnit.AddComponent<AIRootComponent, Node>(AIAutoAttackCenter.Gen());
			
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateBlock(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
		{
	        LSUnit lsUnit = CreateBlock(lsWorld, tableId, teamType);
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
		}
        
        public static LSUnit CreateBlock(LSWorld lsWorld, int tableId, TeamType teamType)
        {
	        TbBlockRow row = TbBlock.Instance.Get(tableId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Block);
	        lsUnit.AddComponent<FlagComponent>();
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        lsUnit.AddComponent<BlockComponent, int>(tableId);
	        lsUnit.AddComponent<PlacementComponent, PlacedLayer, PlacedLayer, bool[]>(PlacedLayer.Block, PlacedLayer.Map | PlacedLayer.Block, row.Shape);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        return lsUnit;
        }
        
        public static LSUnit CreateBuilding(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        LSUnit lsUnit = CreateBuilding(lsWorld, tableId, 1, teamType);
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateBuilding(LSWorld lsWorld, int tableId, int level, TeamType teamType)
        {
	        TbBuildingRow row = TbBuilding.Instance.Get(tableId, level);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();

	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Building);
	        lsUnit.AddComponent<FlagComponent>();
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        lsUnit.AddComponent<BuildingComponent, int, int>(tableId, 1);
	        lsUnit.AddComponent<PlacementComponent, PlacedLayer, PlacedLayer, bool[]>(PlacedLayer.Building, PlacedLayer.Block, row.Shape);

	        PropComponent propComponent = lsUnit.AddComponent<PropComponent>();
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        lsUnit.AddComponent<SkillComponent, int[]>(row.Skills);
	        if (row.ProductSkill > 0) {
		        lsUnit.AddComponent<ProductComponent, int>(row.ProductSkill);
	        }
	        lsUnit.AddComponent<AIRootComponent, Node>(new Sequence(new ActionAttack(), new WaitSecond(FP.Half)));
	        return lsUnit;
        }
        
        public static LSUnit CreateDrop(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Drop);
	        lsUnit.AddComponent<DropComponent, int>(tableId);
	        
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        // 创建指向目标单位的子弹
        public static LSUnit CreateBullet(LSWorld lsWorld, int bulletId, TSVector position, LSUnit caster, LSUnit target)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
			
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.identity);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<TeamComponent, TeamType>(caster.GetComponent<TeamComponent>().Type);
	        BulletComponent bulletComponent = lsUnit.AddComponent<BulletComponent, int, LSUnit, LSUnit>(bulletId, caster, target);

	        lsUnit.AddComponent<TrackComponent, int, LSUnit, TSVector>(bulletComponent.TbBulletRow.TrackId, target, TSVector.zero);
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() { LSUnit = lsUnit });
	        return lsUnit;
        }
        
        // 创建指向目标点的子弹
        public static LSUnit CreateBulletToPosition(LSWorld lsWorld, int bulletId, TSVector position, LSUnit caster, TSVector targetPosition)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
			
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.identity);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<TeamComponent, TeamType>(caster.GetComponent<TeamComponent>().Type);
	        BulletComponent bulletComponent = lsUnit.AddComponent<BulletComponent, int, LSUnit, LSUnit>(bulletId, caster, null);

	        lsUnit.AddComponent<TrackComponent, int, LSUnit, TSVector>(bulletComponent.TbBulletRow.TrackId, null, targetPosition);
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() { LSUnit = lsUnit });
	        return lsUnit;
        }
        
        public static void SummonUnit(LSWorld lsWorld, EUnitType type, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        switch (type)
	        {
		        case EUnitType.Building:
			        CreateBuilding(lsWorld, tableId, position, rotation, teamType);
			        break;
		        case EUnitType.Soldier:
			        CreateSoldier(lsWorld, tableId, position, rotation, teamType);
			        break;
		        case EUnitType.Drop:
			        CreateDrop(lsWorld, tableId, position, rotation, teamType);
			        break;
		        default: break;
	        }
        }

        // 确保实时属性存在
        private static void EnsureRuntimeProp(PropComponent propComponent)
		{
			if (!propComponent.Contains(NumericType.Hp)) {
				propComponent.Set(NumericType.Hp, propComponent.Get(NumericType.MaxHp), false);
			}
			if (!propComponent.Contains(NumericType.Mp)) {
				propComponent.Set(NumericType.Mp, propComponent.Get(NumericType.MaxMp), false);
			}
		}
    }
}
