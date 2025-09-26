using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewEffectComponent))]
    [LSEntitySystemOf(typeof(LSViewEffectComponent))]
    [FriendOf(typeof(LSViewEffectComponent))]
    public static partial class LSViewEffectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewEffectComponent self)
        {

        }

        [EntitySystem]
        private static void Destroy(this LSViewEffectComponent self)
        {

        }
        
        public static async ETTask PlayFx(this LSViewEffectComponent self, int fxId)
        {
            await ETTask.CompletedTask;
            var fxRow = TbSkillResource.Instance.Get(fxId);
            if (fxRow == null)
                return;
            var resourceRow = TbResource.Instance.Get(fxRow.Resource);
            if (resourceRow == null)
                return;
            //
            // LSUnitView lsUnitView = self.LSViewOwner();
            // GameObject prefab = self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetSync<GameObject>(resourceRow.Url);
            // LSViewTransformComponent viewTransformComponent = lsUnitView.GetComponent<LSViewTransformComponent>();
            //
            // Transform transform = viewTransformComponent.GetAttachTransform(fxRow.BindPointType);
            // GameObject go = UnityEngine.Object.Instantiate(prefab, transform);
            //


        }
    }
}