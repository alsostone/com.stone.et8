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
                case EUnitType.Drop:
                    CreateDropView(viewComponent, lsUnit);
                    break;
                case EUnitType.Bullet:
                    CreateBulletView(viewComponent, lsUnit);
                    break;
            }
        }

        private static void CreateHeroView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();

            HeroComponent heroComponent = lsUnit.GetComponent<HeroComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(heroComponent.TbSkinRow.Model);
            GameObject prefab = root.GetComponent<ResourcesLoaderComponent>().LoadAssetSync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            viewComponent.PlayerViews.Add(lsUnitView);
            lsUnitView.AddComponent<LSAnimationComponent>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);
            lsUnitView.AddComponent<LSViewSkillComponent>();

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, Transform, float, float>(Vector3.up * 3.5f, unitGo.transform, hp, hpMax);
            lsUnitView.AddComponent<LSViewGridBuilderComponent>();
        }

        private static void CreateBlockView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();

            BlockComponent blockComponent = lsUnit.GetComponent<BlockComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(blockComponent.TbRow.Model);
            GameObject prefab = root.GetComponent<ResourcesLoaderComponent>().LoadAssetSync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);

            PlacementData placementData = lsUnit.GetComponent<PlacementComponent>().PlacementData;
            lsUnitView.AddComponent<LSViewPlacementComponent, PlacementData>(placementData);
        }

        private static void CreateBuildingView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();

            BuildingComponent buildingComponent = lsUnit.GetComponent<BuildingComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(buildingComponent.TbRow.Model);
            GameObject prefab = root.GetComponent<ResourcesLoaderComponent>().LoadAssetSync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSAnimationComponent>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);
            lsUnitView.AddComponent<LSViewSkillComponent>();
            
            PlacementData placementData = lsUnit.GetComponent<PlacementComponent>().PlacementData;
            lsUnitView.AddComponent<LSViewPlacementComponent, PlacementData>(placementData);
        }

        private static void CreateSoldierView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();

            SoldierComponent soldierComponent = lsUnit.GetComponent<SoldierComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(soldierComponent.TbRow.Model);
            GameObject prefab = root.GetComponent<ResourcesLoaderComponent>().LoadAssetSync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSAnimationComponent>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);
            lsUnitView.AddComponent<LSViewSkillComponent>();

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, Transform, float, float>(Vector3.up * 3.5f, unitGo.transform, hp, hpMax);
        }

        private static void CreateDropView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();

            DropComponent dropComponent = lsUnit.GetComponent<DropComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(dropComponent.TbRow.Model);
            GameObject prefab = root.GetComponent<ResourcesLoaderComponent>().LoadAssetSync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, true);
        }

        private static void CreateBulletView(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();

            BulletComponent bulletComponent = lsUnit.GetComponent<BulletComponent>();
            TbSkillResourceRow row = TbSkillResource.Instance.Get(bulletComponent.TbBulletRow.ResourceId);
            TbResourceRow resourceRow = TbResource.Instance.Get(row.Resource);
            GameObject prefab = root.GetComponent<ResourcesLoaderComponent>().LoadAssetSync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);

            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, true);
        }

    }
}

