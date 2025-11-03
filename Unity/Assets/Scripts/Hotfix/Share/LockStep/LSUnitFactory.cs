using System;
using System.Collections.Generic;
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
		    lsUnit.AddComponent<PropComponent, int>(0);
		    return lsUnit;
	    }

	    public static LSUnit CreateTeam(LSWorld lsWorld, TeamType teamType)
	    {
		    LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
		    LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(LSConstValue.GlobalIdOffset - 1 - (int)teamType);
		    
		    lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Team);
		    lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
		    lsUnit.AddComponent<WorkQueueComponent>();
		    
		    PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(0);
		    propComponent.Set(NumericType.MaxGoldBase, 100, false);
		    propComponent.Set(NumericType.MaxWoodBase, 100, false);
		    propComponent.Set(NumericType.MaxPopulationBase, 20, false);
		    
		    propComponent.Set(NumericType.Gold, 50, false);
		    propComponent.Set(NumericType.Wood, 50, false);
		    propComponent.Set(NumericType.Population, 10, false);
		    
		    return lsUnit;
	    }
	    
	    // 把玩家和英雄拆分的意义：RTS类型的游戏中没有主控单位, 有主控单位时依靠绑定逻辑
	    public static LSUnit CreatePlayer(LSWorld lsWorld, long playerId, TeamType teamType, long bindId = 0)
	    {
		    LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
		    LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(playerId);
		    
		    lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Player);
		    lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
		    lsUnit.AddComponent<PlayerComponent, long>(bindId);
		    lsUnit.AddComponent<LSGridBuilderComponent>();
		    lsUnit.AddComponent<SelectionComponent>();
		    
		    // 初始卡组 可以通过配置表来
		    var cards = ObjectPool.Instance.Fetch<List<LSRandomDropItem>>();
		    cards.Add(new LSRandomDropItem(EUnitType.Building, 30021, 1));
		    cards.Add(new LSRandomDropItem(EUnitType.Block, 2003, 2));
		    cards.Add(new LSRandomDropItem(EUnitType.Item, 5001, 1));
		    cards.Add(new LSRandomDropItem(EUnitType.Item, 5003, 1));
		    cards.Add(new LSRandomDropItem(EUnitType.Item, 5004, 1));
		    cards.Add(new LSRandomDropItem(EUnitType.Item, 5005, 1));
		    lsUnit.AddComponent<CardBagComponent, List<LSRandomDropItem>>(cards);
		    cards.Clear();
		    ObjectPool.Instance.Recycle(cards);
		    
		    lsUnit.AddComponent<CardSelectComponent>();
		    lsUnit.AddComponent<LSCommandsRunComponent>();
			
		    EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
		    return lsUnit;
	    }
	    
        public static LSUnit CreateHero(LSWorld lsWorld, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        TbHeroSkinRow skinRow = TbHeroSkin.Instance.Get(tableId);
	        TbHeroRow row = TbHero.Instance.Get(skinRow.HeroId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Hero);
	        lsUnit.AddComponent<FlagComponent>();
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(row.Radius);
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);
			
	        lsUnit.AddComponent<HeroComponent, int, int>(row.Id, skinRow.Id);
			lsUnit.AddComponent<DeathComponent, bool>(false);
			lsUnit.AddComponent<BuffComponent>();
			lsUnit.AddComponent<BeHitComponent>();
			lsUnit.AddComponent<SkillComponent, int[], int[]>(row.NormalSkills, row.ActiveSkills);
			lsUnit.AddComponent<MovePathFindingComponent, bool>(false);
			
			EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
            return lsUnit;
        }
        
        public static LSUnit CreateSoldier(LSWorld lsWorld, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        TbSoldierRow row = TbSoldier.Instance.Get(tableId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Soldier);
	        lsUnit.AddComponent<FlagComponent>();
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(row.Radius);
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<SoldierComponent, int>(tableId);
	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        lsUnit.AddComponent<SkillComponent, int[], int[]>(row.Skills, null);
	        lsUnit.AddComponent<MoveFlowFieldComponent>();
	        lsUnit.AddComponent<AIRootComponent, Node>(teamType == TeamType.TeamA ? AIAutoAttack.Gen() : AIAutoAttackCenter.Gen());

	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateBlock(LSWorld lsWorld, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        TbBlockRow row = TbBlock.Instance.Get(tableId);
	        PlacementData placementData = new PlacementData();
	        placementData.placementType = PlacedLayer.Block;
	        placementData.placedLayer = (PlacedLayer)row.PlacedLayer;
	        placementData.rotation = 0;
	        placementData.Rotation(row.Shape, placementData.points, angle / 90);
	        
	        // 判断是否能够放置到网格 不能则不创建
	        LSGridMapComponent lsGridMapComponent = lsWorld.GetComponent<LSGridMapComponent>();
	        IndexV2 index = lsGridMapComponent.ConvertToIndex(position);
	        if (!lsGridMapComponent.CanPut(index.x, index.z, placementData))
		        return null;

	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        placementData.id = lsUnit.Id;
	        lsGridMapComponent.Put(index.x, index.z, placementData);
	        
	        TSVector pos = lsGridMapComponent.GetPutPosition(placementData);
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(pos, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Block);
	        lsUnit.AddComponent<FlagComponent>();
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        lsUnit.AddComponent<PlacementComponent, PlacementData>(placementData);
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(5000);
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<BlockComponent, int>(tableId);
	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateBuilding(LSWorld lsWorld, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        TbBuildingRow row = TbBuilding.Instance.Get(tableId);
	        PlacementData placementData = new PlacementData();
	        placementData.placementType = PlacedLayer.Building;
	        placementData.placedLayer = (PlacedLayer)row.PlacedLayer;
	        placementData.rotation = 0;
	        placementData.Rotation(row.Shape, placementData.points, angle / 90);
	        
	        // 判断是否能够放置到网格 不能则不创建
	        LSGridMapComponent lsGridMapComponent = lsWorld.GetComponent<LSGridMapComponent>();
	        IndexV2 index = lsGridMapComponent.ConvertToIndex(position);
	        if (!lsGridMapComponent.CanPut(index.x, index.z, placementData))
		        return null;
	        
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        placementData.id = lsUnit.Id;
	        lsGridMapComponent.Put(index.x, index.z, placementData);

	        TSVector pos = lsGridMapComponent.GetPutPosition(placementData);
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(pos, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Building);
	        lsUnit.AddComponent<FlagComponent>();
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        lsUnit.AddComponent<PlacementComponent, PlacementData>(placementData);

	        PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(row.Radius);
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<BuildingComponent, int>(tableId);
	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        lsUnit.AddComponent<SkillComponent, int[], int[]>(row.Skills, null);
	        if (row.ProductSkill > 0) {
		        lsUnit.AddComponent<ProductComponent, int>(row.ProductSkill);
	        }
	        lsUnit.AddComponent<AIRootComponent, Node>(new Sequence(new RequestAttack(), new WaitSecond(FP.Half)));
	        
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateItem(LSWorld lsWorld, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Item);
	        lsUnit.AddComponent<TeamComponent, TeamType>(teamType);
	        lsUnit.AddComponent<ItemComponent, int>(tableId);
	        
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
        
        public static void SummonUnit(LSWorld lsWorld, EUnitType type, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        switch (type)
	        {
		        case EUnitType.Building:
			        CreateBuilding(lsWorld, tableId, position, angle, teamType);
			        break;
		        case EUnitType.Soldier:
			        CreateSoldier(lsWorld, tableId, position, angle, teamType);
			        break;
		        case EUnitType.Item:
			        CreateItem(lsWorld, tableId, position, angle, teamType);
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
