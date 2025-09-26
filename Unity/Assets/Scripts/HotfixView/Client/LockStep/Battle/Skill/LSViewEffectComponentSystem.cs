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
            
            LSUnitView lsUnitView = self.LSViewOwner();
            LSViewTransformComponent viewTransformComponent = lsUnitView.GetComponent<LSViewTransformComponent>();
            Transform attachTransform = viewTransformComponent.GetAttachTransform(fxRow.BindPointType);
            
            // ResourcesPoolComponent poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            // GameObject prefab = await poolComponent.FetchAsync(fxRow.Resource, attachTransform);
            //
            


        }
    }
}