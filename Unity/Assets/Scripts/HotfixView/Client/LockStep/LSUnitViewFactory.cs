using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(TypeComponent))]
    [FriendOf(typeof(LSOperaDragComponent))]
    public static class LSUnitViewFactory
    {
        public static void CreateLSUnitView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            var type = lsUnit.GetComponent<TypeComponent>().Type;
            switch (type)
            {
                case EUnitType.Hero:
                    CreateHeroView(viewComponent, lsUnit);
                    break;
                case EUnitType.Block:
                    CreateBlockView(viewComponent, lsUnit);
                    break;
                case EUnitType.Building:
                    CreateBuildingView(viewComponent, lsUnit);
                    break;
                case EUnitType.Soldier:
                    CreateSoldierView(viewComponent, lsUnit);
                    break;
                case EUnitType.Item:
                    CreateItemView(viewComponent, lsUnit);
                    break;
                case EUnitType.Bullet:
                    CreateBulletView(viewComponent, lsUnit);
                    break;
                case EUnitType.Player:
                    CreatePlayerView(viewComponent, lsUnit);
                    break;
            }
        }
        
        // 把玩家和英雄拆分的意义：RTS类型的游戏中没有主控单位, 有主控英雄时依靠绑定逻辑
        private static void CreatePlayerView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            ET.PlayerComponent playerComponent = lsUnit.GetComponent<ET.PlayerComponent>();
            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, null);
            lsUnitView.AddComponent<LSViewPlayerComponent, long>(playerComponent.BindEntityId);
            lsUnitView.AddComponent<LSViewGridBuilderComponent>();
            
            lsUnitView.AddComponent<LSViewCardBagComponent>();
            lsUnitView.AddComponent<LSViewCardSelectComponent>();
        }

        private static void CreateHeroView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            HeroComponent heroComponent = lsUnit.GetComponent<HeroComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(heroComponent.TbSkinRow.Model, globalComponent.Unit, true);
            
            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            Animation animation = unitGo.GetComponent<Animation>();
            if (animation) {
                lsUnitView.AddComponent<LSAnimationComponent, Animation>(animation);
            }
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);
            lsUnitView.AddComponent<LSViewSkillComponent>();
            lsUnitView.AddComponent<LSViewEffectComponent>();

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, Transform, float, float>(Vector3.up * 3.5f, unitGo.transform, hp, hpMax);
        }

        private static void CreateBlockView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            BlockComponent blockComponent = lsUnit.GetComponent<BlockComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(blockComponent.TbRow.Model, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);
            lsUnitView.AddComponent<LSViewEffectComponent>();

            PlacementData placementData = lsUnit.GetComponent<PlacementComponent>().PlacementData;
            lsUnitView.AddComponent<LSViewPlacementComponent, PlacementData>(placementData);
        }

        private static void CreateBuildingView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            BuildingComponent buildingComponent = lsUnit.GetComponent<BuildingComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(buildingComponent.TbRow.Model, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            Animation animation = unitGo.GetComponent<Animation>();
            if (animation) {
                lsUnitView.AddComponent<LSAnimationComponent, Animation>(animation);
            }
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);
            lsUnitView.AddComponent<LSViewSkillComponent>();
            lsUnitView.AddComponent<LSViewEffectComponent>();
            
            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, Transform, float, float>(Vector3.up * 3.5f, unitGo.transform, hp, hpMax);
            
            PlacementData placementData = lsUnit.GetComponent<PlacementComponent>().PlacementData;
            lsUnitView.AddComponent<LSViewPlacementComponent, PlacementData>(placementData);
        }

        private static void CreateSoldierView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            SoldierComponent soldierComponent = lsUnit.GetComponent<SoldierComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(soldierComponent.TbRow.Model, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            Animation animation = unitGo.GetComponent<Animation>();
            if (animation) {
                lsUnitView.AddComponent<LSAnimationComponent, Animation>(animation);
            }
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);
            lsUnitView.AddComponent<LSViewSkillComponent>();
            lsUnitView.AddComponent<LSViewEffectComponent>();

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, Transform, float, float>(Vector3.up * 3.5f, unitGo.transform, hp, hpMax);
        }

        private static void CreateItemView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            ItemComponent itemComponent = lsUnit.GetComponent<ItemComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(itemComponent.TbRow.Model, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, true);
        }

        private static void CreateBulletView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            BulletComponent bulletComponent = lsUnit.GetComponent<BulletComponent>();
            TbSkillResourceRow row = TbSkillResource.Instance.Get(bulletComponent.TbBulletRow.ResourceId);
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(row.Resource, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, true);
        }

    }
}

