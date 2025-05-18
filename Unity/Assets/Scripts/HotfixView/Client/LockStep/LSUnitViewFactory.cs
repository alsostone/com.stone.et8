using UnityEngine;

namespace ET.Client
{
    public static class LSUnitViewFactory
    {
        public static async ETTask CreateLSUnitViewAsync(Room room, LSWorld lsWorld, LSUnit lsUnit)
        {
            var type = lsUnit.GetComponent<TypeComponent>().GetUnitType();
            switch (type)
            {
                case EUnitType.Hero:
                    await CreateHeroViewAsync(room, lsWorld, lsUnit);
                    break;
                case EUnitType.Soldier:
                    await CreateHeroViewAsync(room, lsWorld, lsUnit);
                    break;
                case EUnitType.Bullet:
                    await CreateBulletViewAsync(room, lsWorld, lsUnit);
                    break;
            }
        }

        private static async ETTask CreateBulletViewAsync(Room room, LSWorld lsWorld, LSUnit lsUnit)
        {
            throw new System.NotImplementedException();
        }

        private static async ETTask CreateHeroViewAsync(Room room, LSWorld lsWorld, LSUnit lsUnit)
        {
            Vector3 position = lsUnit.Position.ToVector();
            
            Scene root = lsWorld.Root();
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await root.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            unitGo.transform.position = position;

            LSUnitView lsUnitView = room.GetComponent<LSUnitViewComponent>().AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSAnimatorComponent>();

            var propComponent = lsUnit.GetComponent<PropComponent>();
            float hp = propComponent.Get(NumericType.Hp).AsFloat();
            float hpMax = propComponent.Get(NumericType.MaxHp).AsFloat();
            lsUnitView.AddComponent<LSHudComponent, Vector3, Transform, float, float>(Vector3.up * 1.75f, unitGo.transform, hp, hpMax);
        }
    }
}

