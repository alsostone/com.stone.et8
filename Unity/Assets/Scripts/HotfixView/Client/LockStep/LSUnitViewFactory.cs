using ST.GridBuilder;
using TrueSync;
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
            lsUnitView.AddComponent<LSViewSelectionComponent>();
            
            lsUnitView.AddComponent<LSViewCardBagComponent>();
            lsUnitView.AddComponent<LSViewCardSelectComponent>();
        }

        private static void CreateHeroView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Room room = viewComponent.Room();
            HeroComponent heroComponent = lsUnit.GetComponent<HeroComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = room.GetComponent<ResourcesPoolComponent>().Fetch(heroComponent.TbSkinRow.Model, globalComponent.Unit, true);
            
            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            Animation animation = unitGo.GetComponent<Animation>();
            if (animation) {
                lsUnitView.AddComponent<LSAnimationComponent, Animation, float>(animation, room.TimeScale);
            }
            
            AttachPointCollector collector = unitGo.GetComponent<AttachPointCollector>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, AttachPointCollector, bool>(unitGo.transform, collector, false);
            lsUnitView.AddComponent<LSViewSkillComponent>();
            lsUnitView.AddComponent<ViewEffectComponent, float>(room.TimeScale);

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, float, float>(Vector3.zero, hp, hpMax);
        }

        private static void CreateBlockView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Room room = viewComponent.Room();
            BlockComponent blockComponent = lsUnit.GetComponent<BlockComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = room.GetComponent<ResourcesPoolComponent>().Fetch(blockComponent.TbRow.Model, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            
            AttachPointCollector collector = unitGo.GetComponent<AttachPointCollector>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, AttachPointCollector, bool>(unitGo.transform, collector, false);
            lsUnitView.AddComponent<LSViewPlacementComponent>();
            lsUnitView.AddComponent<ViewEffectComponent, float>(room.TimeScale);
        }

        private static void CreateBuildingView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Room room = viewComponent.Room();
            BuildingComponent buildingComponent = lsUnit.GetComponent<BuildingComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = room.GetComponent<ResourcesPoolComponent>().Fetch(buildingComponent.TbRow.Model, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            Animation animation = unitGo.GetComponent<Animation>();
            if (animation) {
                lsUnitView.AddComponent<LSAnimationComponent, Animation, float>(animation, room.TimeScale);
            }
            
            AttachPointCollector collector = unitGo.GetComponent<AttachPointCollector>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, AttachPointCollector, bool>(unitGo.transform, collector, false);
            lsUnitView.AddComponent<LSViewPlacementComponent>();
            lsUnitView.AddComponent<LSViewSkillComponent>();
            lsUnitView.AddComponent<ViewEffectComponent, float>(room.TimeScale);
            
            var propComponent = lsUnit.GetComponent<PropComponent>();
            if (buildingComponent.TbRow.RangeIndicator > 0) {
                float range = propComponent.Get(NumericType.AtkRange).AsFloat();
                lsUnitView.AddComponent<ViewIndicatorComponent, int, float>(buildingComponent.TbRow.RangeIndicator, range);
            }
            FP hpMax = propComponent.Get(NumericType.MaxHp);
            if (hpMax > FP.Epsilon) {
                float hp = propComponent.Get(NumericType.Hp).AsFloat();
                lsUnitView.AddComponent<LSViewHudComponent, Vector3, float, float>(Vector3.zero, hp, hpMax.AsFloat());
            }
        }

        private static void CreateSoldierView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Room room = viewComponent.Room();
            SoldierComponent soldierComponent = lsUnit.GetComponent<SoldierComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(soldierComponent.TbRow.Model, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            Animation animation = unitGo.GetComponent<Animation>();
            if (animation) {
                lsUnitView.AddComponent<LSAnimationComponent, Animation, float>(animation, room.TimeScale);
            }
            
            AttachPointCollector collector = unitGo.GetComponent<AttachPointCollector>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, AttachPointCollector, bool>(unitGo.transform, collector, false);
            lsUnitView.AddComponent<LSViewSkillComponent>();
            lsUnitView.AddComponent<ViewEffectComponent, float>(room.TimeScale);

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, float, float>(Vector3.zero, hp, hpMax);
        }

        private static void CreateItemView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            ItemComponent itemComponent = lsUnit.GetComponent<ItemComponent>();
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(itemComponent.TbRow.Model, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, AttachPointCollector, bool>(unitGo.transform, null, true);
        }

        private static void CreateBulletView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            BulletComponent bulletComponent = lsUnit.GetComponent<BulletComponent>();
            TbSkillResourceRow row = TbSkillResource.Instance.Get(bulletComponent.TbBulletRow.ResourceId);
            GlobalComponent globalComponent = viewComponent.Root().GetComponent<GlobalComponent>();
            GameObject unitGo = viewComponent.Room().GetComponent<ResourcesPoolComponent>().Fetch(row.Resource, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, AttachPointCollector, bool>(unitGo.transform, null, true);
        }

    }
}

