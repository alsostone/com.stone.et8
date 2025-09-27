using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(EffectViewComponent))]
    [LSEntitySystemOf(typeof(EffectViewComponent))]
    [FriendOf(typeof(EffectViewComponent))]
    public static partial class EffectViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this EffectViewComponent self)
        {

        }

        [EntitySystem]
        private static void Destroy(this EffectViewComponent self)
        {
            self.Effects.Clear();
        }
        
        public static async ETTask PlayFx(this EffectViewComponent self, int fxId)
        {
            EffectView view = null;
            if (self.Effects.TryGetValue(fxId, out var @ref)) {
                view = @ref;
                if (view != null) {
                    view.Reset();
                    return;
                }
                self.Effects.Remove(fxId);
            }
            
            var fxRow = TbSkillResource.Instance.Get(fxId);
            if (fxRow == null) { return; }

            LSUnitView lsUnitView = self.LSViewOwner();
            LSViewTransformComponent viewTransformComponent = lsUnitView.GetComponent<LSViewTransformComponent>();
            Transform attachTransform = viewTransformComponent.GetAttachTransform(fxRow.BindPointType);

            ResourcesPoolComponent poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            GameObject go = await poolComponent.FetchAsync(fxRow.Resource, attachTransform);
            view = self.AddChild<EffectView, GameObject>(go);
            self.Effects.Add(fxId, view);
        }
    }
}