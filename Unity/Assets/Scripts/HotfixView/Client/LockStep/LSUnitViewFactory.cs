using UnityEngine;

namespace ET.Client
{
    public static class LSUnitViewFactory
    {
        public static async ETTask CreateLSUnitViewAsync(Scene scene, LSUnit lsUnit)
        {
            var type = lsUnit.GetComponent<TypeComponent>().GetUnitType();
            switch (type)
            {
                case EUnitType.Hero:
                    await CreateHeroViewAsync(scene, lsUnit);
                    break;
                case EUnitType.Bullet:
                    await CreateBulletViewAsync(scene, lsUnit);
                    break;
            }
        }

        private static async ETTask CreateBulletViewAsync(Scene scene, LSUnit lsUnit)
        {
            throw new System.NotImplementedException();
        }

        private static async ETTask CreateHeroViewAsync(Scene scene, LSUnit lsUnit)
        {
            Scene root = scene.Root();
            Room room = scene.Room();
            string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            GameObject bundleGameObject = await room.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(assetsName);
            GameObject prefab = bundleGameObject.Get<GameObject>("Skeleton");

            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject unitGo = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            unitGo.transform.position = lsUnit.Position.ToVector();

            LSUnitView lsUnitView = room.GetComponent<LSUnitViewComponent>().AddChildWithId<LSUnitView, GameObject>(lsUnit.Id, unitGo);
            lsUnitView.AddComponent<LSAnimatorComponent>();
        }
    }
}

