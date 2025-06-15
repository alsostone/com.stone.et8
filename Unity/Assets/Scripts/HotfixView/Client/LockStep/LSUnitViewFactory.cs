using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(TypeComponent))]
    public static class LSUnitViewFactory
    {
        public static async ETTask CreateLSUnitViewAsync(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            var type = lsUnit.GetComponent<TypeComponent>().Type;
            switch (type)
            {
                case EUnitType.Hero:
                    await CreateHeroViewAsync(viewComponent, lsUnit);
                    break;
                case EUnitType.Building:
                    await CreateBuildingViewAsync(viewComponent, lsUnit);
                    break;
                case EUnitType.Soldier:
                    await CreateSoldierViewAsync(viewComponent, lsUnit);
                    break;
                case EUnitType.Drop:
                    await CreateDropViewAsync(viewComponent, lsUnit);
                    break;
                case EUnitType.Bullet:
                    await CreateBulletViewAsync(viewComponent, lsUnit);
                    break;
            }
        }

        private static async ETTask CreateHeroViewAsync(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();
            
            HeroComponent heroComponent = lsUnit.GetComponent<HeroComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(heroComponent.TbSkinRow.Model);
            GameObject prefab = await root.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            
            // 回滚后重新执行帧，ID对应的实体可能已变化。需清除表现层单位重新创建
            viewComponent.RemoveChild(lsUnit.Id);
            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSAnimatorComponent>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, Transform, float, float>(Vector3.up * 1.75f, unitGo.transform, hp, hpMax);
        }
        
        private static async ETTask CreateBuildingViewAsync(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();
            
            BuildingComponent buildingComponent = lsUnit.GetComponent<BuildingComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(buildingComponent.TbRow.Model);
            GameObject prefab = await root.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            
            // 回滚后重新执行帧，ID对应的实体可能已变化。需清除表现层单位重新创建
            viewComponent.RemoveChild(lsUnit.Id);
            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSAnimatorComponent>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);
        }
        
        private static async ETTask CreateSoldierViewAsync(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();
            
            SoldierComponent soldierComponent = lsUnit.GetComponent<SoldierComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(soldierComponent.TbRow.Model);
            GameObject prefab = await root.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            
            // 回滚后重新执行帧，ID对应的实体可能已变化。需清除表现层单位重新创建
            viewComponent.RemoveChild(lsUnit.Id);
            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSAnimatorComponent>();
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, false);

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSViewHudComponent, Vector3, Transform, float, float>(Vector3.up * 1.75f, unitGo.transform, hp, hpMax);
        }
        
        private static async ETTask CreateDropViewAsync(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();
            
            DropComponent dropComponent = lsUnit.GetComponent<DropComponent>();
            TbResourceRow resourceRow = TbResource.Instance.Get(dropComponent.TbRow.Model);
            GameObject prefab = await root.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            
            // 回滚后重新执行帧，ID对应的实体可能已变化。需清除表现层单位重新创建
            viewComponent.RemoveChild(lsUnit.Id);
            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, true);
        }
        
        private static async ETTask CreateBulletViewAsync(LSUnitViewComponent viewComponent, LSUnit lsUnit)
        {
            Scene root = viewComponent.Root();

            BulletComponent bulletComponent = lsUnit.GetComponent<BulletComponent>();
            TbSkillResourceRow row = TbSkillResource.Instance.Get(bulletComponent.TbBulletRow.ResourceId);
            TbResourceRow resourceRow = TbResource.Instance.Get(row.Resource);
            GameObject prefab = await root.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(resourceRow.Url);

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            
            // 回滚后重新执行帧，ID对应的实体可能已变化。需清除表现层单位重新创建
            viewComponent.RemoveChild(lsUnit.Id);
            LSUnitView lsUnitView = viewComponent.AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSViewTransformComponent, Transform, bool>(unitGo.transform, true);
        }

    }
}

