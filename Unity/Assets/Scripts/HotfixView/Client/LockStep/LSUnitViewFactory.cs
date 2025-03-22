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
            Scene root = lsWorld.Root();
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await root.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            unitGo.transform.position = lsUnit.Position.ToVector();

            LSUnitView lsUnitView = room.GetComponent<LSUnitViewComponent>().AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSAnimatorComponent>();
        }
    }
}

