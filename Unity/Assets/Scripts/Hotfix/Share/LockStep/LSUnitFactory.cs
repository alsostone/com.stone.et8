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

	    public static LSUnit CreateTeamPlayer(LSWorld lsWorld, TeamType teamType)
	    {
		    LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
		    LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(LSConstValue.GlobalIdOffset - 1 - (int)teamType);
		    
		    lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Team);
		    lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, 0);
		    lsUnit.AddComponent<PropComponent, int>(0);
		    lsUnit.AddComponent<ContainerComponent>();
		    
		    lsUnit.AddComponent<WorkQueueComponent>();
		    
		    return lsUnit;
	    }
	    
	    public static LSUnit CreateTeamMonster(LSWorld lsWorld, TeamType teamType)
	    {
		    LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
		    LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(LSConstValue.GlobalIdOffset - 1 - (int)teamType);
		    
		    lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Team);
		    lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, 0);
		    lsUnit.AddComponent<PropComponent, int>(0);
		    lsUnit.AddComponent<ContainerComponent>();
		    
		    return lsUnit;
	    }
	    
	    // 把玩家和英雄拆分的意义：RTS类型的游戏中没有主控单位, 有主控单位时依靠绑定逻辑
	    public static LSUnit CreatePlayer(LSWorld lsWorld, long playerId, TeamType teamType)
	    {
		    LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
		    LSUnit lsUnit = lsUnitComponent.AddChildWithId<LSUnit>(playerId);
		    
		    lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Player);
		    lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, 0);
		    lsUnit.AddComponent<PropComponent, int>(0);
		    
		    lsUnit.AddComponent<PlayerComponent>();
		    lsUnit.AddComponent<CardBagComponent>();
		    lsUnit.AddComponent<CardSelectComponent>();
		    
		    lsUnit.AddComponent<LSGridBuilderComponent>();
		    lsUnit.AddComponent<SelectionComponent>();
		    lsUnit.AddComponent<LSCommandsRunComponent>();
			
		    EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
		    return lsUnit;
	    }
	    
        public static LSUnit CreateHero(LSUnit lsOwner, int tableId, TSVector position, TSQuaternion rotation, TeamType teamType)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        TbHeroSkinRow skinRow = TbHeroSkin.Instance.Get(tableId);
	        TbHeroRow row = TbHero.Instance.Get(skinRow.HeroId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = tableId;

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Hero);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, lsOwner.Id);
	        lsUnit.AddComponent<TargetableComponent>();
	        lsUnit.AddComponent<FlagComponent>();
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(row.Radius);
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);
			
	        lsUnit.AddComponent<HeroComponent, int, int>(row.Id, skinRow.Id);
			lsUnit.AddComponent<DeathComponent, bool>(false);
			lsUnit.AddComponent<BuffComponent>();
			lsUnit.AddComponent<BeHitComponent>();
			lsUnit.AddComponent<MoveFlowFieldComponent>();
			lsUnit.AddComponent<SkillComponent, int[], int[]>(row.NormalSkills, row.ActiveSkills);
			
			EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
            return lsUnit;
        }
        
        public static LSUnit CreateSoldier(LSUnit lsOwner, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        TbSoldierRow row = TbSoldier.Instance.Get(tableId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = tableId;

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Soldier);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, lsOwner.Id);
	        lsUnit.AddComponent<TargetableComponent>();
	        FlagComponent flagComponent = lsUnit.AddComponent<FlagComponent>();
	        
	        PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(row.Radius);
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<SoldierComponent, int>(tableId);
	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        lsUnit.AddComponent<MoveFlowFieldComponent>();
	        
	        if (row.Skills != null && row.Skills.Length > 0) {
		        lsUnit.AddComponent<SkillComponent, int[], int[]>(row.Skills, null);
	        } else {
		        // 小兵支持不配置技能 如塔防玩法中的怪只冲击基地
		        flagComponent.AddRestrict((int)FlagRestrict.NotAIAlert);
	        }
	        AIWorldComponent aiWorldComponent = lsWorld.GetComponent<AIWorldComponent>();
	        Node node = aiWorldComponent.GenAINode(row.AiName);
	        if (node != null) {
		        lsUnit.AddComponent<AIRootComponent, Node>(node);
	        }

	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateTrap(LSUnit lsOwner, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        TbTrapRow row = TbTrap.Instance.Get(tableId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = tableId;

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Trap);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, lsOwner.Id);

	        PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(0);
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        if (row.Skills != null && row.Skills.Length > 0) {
		        lsUnit.AddComponent<SkillComponent, int[], int[]>(row.Skills, null);
	        }
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateBlock(LSUnit lsOwner, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
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
	        lsUnit.TableId = tableId;
	        
	        placementData.id = lsUnit.Id;
	        placementData.x = index.x;
	        placementData.z = index.z;
	        
	        TSVector pos = lsGridMapComponent.GetPutPosition(placementData);
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(pos, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Block);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, lsOwner.Id);
	        lsUnit.AddComponent<TargetableComponent>();
	        lsUnit.AddComponent<FlagComponent, int>((int)FlagRestrict.NotRotate);

	        PropComponent propComponent = lsUnit.AddComponent<PropComponent, int>(5000);
	        foreach (var prop in row.Props) {
		        propComponent.Set(prop.Key, (FP)prop.Value / LSConstValue.PropValueScale, false);
	        }
	        EnsureRuntimeProp(propComponent);

	        lsUnit.AddComponent<BlockComponent, int>(tableId);
	        lsUnit.AddComponent<DeathComponent, bool>(true);
	        lsUnit.AddComponent<BuffComponent>();
	        lsUnit.AddComponent<BeHitComponent>();
	        lsUnit.AddComponent<PlacementComponent, PlacementData>(placementData);
	        
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateBuilding(LSUnit lsOwner, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
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
	        lsUnit.TableId = tableId;
	        
	        placementData.id = lsUnit.Id;
	        placementData.x = index.x;
	        placementData.z = index.z;

	        TSVector pos = lsGridMapComponent.GetPutPosition(placementData);
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(pos, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Building);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, lsOwner.Id);
	        lsUnit.AddComponent<TargetableComponent>();
	        lsUnit.AddComponent<FlagComponent, int>((int)FlagRestrict.NotRotate);

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
	        lsUnit.AddComponent<PlacementComponent, PlacementData>(placementData);
	        if (row.ProductSkill > 0) {
		        lsUnit.AddComponent<ProductComponent, int>(row.ProductSkill);
	        }
	        lsUnit.AddComponent<AIRootComponent, Node>(new Sequence(new RequestAttack(), new WaitSecond(FP.Half)));
	        
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        public static LSUnit CreateItem(LSUnit lsOwner, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = tableId;

	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.Euler(0, angle, 0));
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Item);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(teamType, lsOwner.Id);
	        lsUnit.AddComponent<ItemComponent, int>(tableId);
	        
	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
	        return lsUnit;
        }
        
        // 创建固定朝向型子弹（波浪型）非碰撞检测（一次直线索敌，再根据距离判定命中，性能高）
        public static LSUnit CreateBulletToDirection(LSUnit lsOwner, int bulletId, int searchId, TSVector position, TSQuaternion rotation)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        TbBulletRow row = TbBullet.Instance.Get(bulletId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = bulletId;

	        TeamComponent casterTeamComponent = lsOwner.GetComponent<TeamComponent>();
	        TransformComponent thisTransform = lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(casterTeamComponent.Type, casterTeamComponent.OwnerId);
	        
	        // 创建子弹时把目标搜索出来 子弹决定命中时机（通过距离判定，非碰撞检测）
	        List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
	        FP distance = TargetSearcher.Search(searchId, lsOwner, thisTransform.Position, thisTransform.Forward, thisTransform.Upwards, targets);
	        targets.Sort((x, y) => x.SqrDistance.CompareTo(y.SqrDistance));
	        lsUnit.AddComponent<TrackComponent, int, int, int, FP>(row.HorSpeed, row.ControlFactor, row.ControlHeight, distance);
	        lsUnit.AddComponent<BulletComponent, int, LSUnit, List<SearchUnit>>(bulletId, lsOwner, targets);
	        targets.Clear();
	        ObjectPool.Instance.Recycle(targets);

	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() { LSUnit = lsUnit });
	        return lsUnit;
        }
        
        // 创建固定朝向型子弹（波浪型）碰撞检测版本
        public static LSUnit CreateBulletToDirection2(LSUnit lsOwner, int bulletId, TSVector position, TSVector targetPosition)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        TbBulletRow row = TbBullet.Instance.Get(bulletId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = bulletId;

	        TeamComponent casterTeamComponent = lsOwner.GetComponent<TeamComponent>();
	        TSQuaternion rotation = TSQuaternion.LookRotation(targetPosition - position, lsOwner.GetComponent<TransformComponent>().Upwards);
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(casterTeamComponent.Type, casterTeamComponent.OwnerId);
	        
	        lsUnit.AddComponent<TrackComponent, int, int, int, TSVector>(row.HorSpeed, row.ControlFactor, row.ControlHeight, targetPosition);
	        lsUnit.AddComponent<CollisionComponent, int>(2);
	        lsUnit.AddComponent<BulletComponent, ETrackTowardType, int, LSUnit>(ETrackTowardType.Direction2, bulletId, lsOwner);

	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() { LSUnit = lsUnit });
	        return lsUnit;
        }
        
        // 创建固定朝向型子弹（波浪型）碰撞检测版本
        public static LSUnit CreateBulletToDirection2(LSUnit lsOwner, int bulletId, TSVector position, TSQuaternion rotation, FP distance)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        TbBulletRow row = TbBullet.Instance.Get(bulletId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = bulletId;

	        TeamComponent casterTeamComponent = lsOwner.GetComponent<TeamComponent>();
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, rotation);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(casterTeamComponent.Type, casterTeamComponent.OwnerId);
	        
	        lsUnit.AddComponent<TrackComponent, int, int, int, FP>(row.HorSpeed, row.ControlFactor, row.ControlHeight, distance);
	        lsUnit.AddComponent<CollisionComponent, int>(2);
	        lsUnit.AddComponent<BulletComponent, ETrackTowardType, int, LSUnit>(ETrackTowardType.Direction2, bulletId, lsOwner);

	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() { LSUnit = lsUnit });
	        return lsUnit;
        }

        // 创建跟随目标单位的子弹
        public static LSUnit CreateBulletFollowTarget(LSUnit lsOwner, int bulletId, TSVector position, LSUnit target)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        TbBulletRow row = TbBullet.Instance.Get(bulletId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = bulletId;
	        
	        TeamComponent casterTeamComponent = lsOwner.GetComponent<TeamComponent>();
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.identity);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(casterTeamComponent.Type, casterTeamComponent.OwnerId);
	        
	        lsUnit.AddComponent<TrackComponent, int, int, int, LSUnit>(row.HorSpeed, row.ControlFactor, row.ControlHeight, target);
	        lsUnit.AddComponent<BulletComponent, int, LSUnit, LSUnit>(bulletId, lsOwner, target);

	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() { LSUnit = lsUnit });
	        return lsUnit;
        }
        
        // 创建飞向固定位置的子弹
        public static LSUnit CreateBulletToPosition(LSUnit lsOwner, int bulletId, TSVector position, TSVector targetPosition)
        {
	        LSWorld lsWorld = lsOwner.LSWorld();
	        TbBulletRow row = TbBullet.Instance.Get(bulletId);
	        LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
	        LSUnit lsUnit = lsUnitComponent.AddChild<LSUnit>();
	        lsUnit.TableId = bulletId;
	        
	        TeamComponent casterTeamComponent = lsOwner.GetComponent<TeamComponent>();
	        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(position, TSQuaternion.identity);
	        lsUnit.AddComponent<TypeComponent, EUnitType>(EUnitType.Bullet);
	        lsUnit.AddComponent<TeamComponent, TeamType, long>(casterTeamComponent.Type, casterTeamComponent.OwnerId);
	        
	        lsUnit.AddComponent<TrackComponent, int, int, int, TSVector>(row.HorSpeed, row.ControlFactor, row.ControlHeight, targetPosition);
	        lsUnit.AddComponent<BulletComponent, ETrackTowardType, int, LSUnit>(ETrackTowardType.Position, bulletId, lsOwner);

	        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() { LSUnit = lsUnit });
	        return lsUnit;
        }
        
        public static void SummonUnit(LSUnit lsOwner, EUnitType type, int tableId, TSVector position, int angle, TeamType teamType)
        {
	        switch (type)
	        {
		        case EUnitType.Block:
			        CreateBlock(lsOwner, tableId, position, angle, teamType);
			        break;
		        case EUnitType.Building:
			        CreateBuilding(lsOwner, tableId, position, angle, teamType);
			        break;
		        case EUnitType.Soldier:
			        CreateSoldier(lsOwner, tableId, position, angle, teamType);
			        break;
		        case EUnitType.Item:
			        CreateItem(lsOwner, tableId, position, angle, teamType);
			        break;
		        case EUnitType.Trap:
			        CreateTrap(lsOwner, tableId, position, angle, teamType);
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
