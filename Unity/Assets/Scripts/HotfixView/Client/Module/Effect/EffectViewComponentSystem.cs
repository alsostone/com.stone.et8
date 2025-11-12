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
            self.idGenerator = 0;
        }

        [EntitySystem]
        private static void Destroy(this EffectViewComponent self)
        {
            self.SkillEffectViews.Clear();
            foreach (var @ref in self.EffectViews)
            {
                EffectView view = @ref.Value;
                view?.Dispose();
            }
            self.EffectViews.Clear();
        }
        
        private static long GetId(this EffectViewComponent self)
        {
            return ++self.idGenerator;
        }
        
        public static async ETTask PlayFx(this EffectViewComponent self, int fxId)
        {
            EffectView view = null;
            if (self.SkillEffectViews.TryGetValue(fxId, out var @ref)) {
                view = @ref;
                if (view != null) {
                    view.Reset();
                    return;
                }
                self.SkillEffectViews.Remove(fxId);
            }
            
            var fxRow = TbSkillResource.Instance.Get(fxId);
            if (fxRow == null) { return; }

            LSUnitView lsUnitView = self.LSViewOwner();
            LSViewTransformComponent viewTransformComponent = lsUnitView.GetComponent<LSViewTransformComponent>();
            Transform attachTransform = viewTransformComponent.GetAttachTransform((AttachPoint)fxRow.AttachPoint);

            ResourcesPoolComponent poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            GameObject go = await poolComponent.FetchAsync(fxRow.Resource, attachTransform);
            view = self.AddChildWithId<EffectView, GameObject>(self.GetId(), go);
            self.SkillEffectViews.Add(fxId, view);
        }
        
        public static async ETTask PlayFx(this EffectViewComponent self, int fxResource, AttachPoint attachPoint)
        {
            EffectView view = null;
            if (self.EffectViews.TryGetValue(fxResource, out var @ref)) {
                view = @ref;
                if (view != null) {
                    view.Reset();
                    return;
                }
                self.EffectViews.Remove(fxResource);
            }
            
            LSUnitView lsUnitView = self.LSViewOwner();
            LSViewTransformComponent viewTransformComponent = lsUnitView.GetComponent<LSViewTransformComponent>();
            Transform attachTransform = viewTransformComponent.GetAttachTransform(attachPoint);

            ResourcesPoolComponent poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            GameObject go = await poolComponent.FetchAsync(fxResource, attachTransform);
            view = self.AddChildWithId<EffectView, GameObject>(self.GetId(), go);
            self.EffectViews.Add(fxResource, view);
        }
        
        public static void StopFx(this EffectViewComponent self, int fxResource)
        {
            if (self.EffectViews.TryGetValue(fxResource, out var @ref)) {
                EffectView view = @ref;
                view?.Dispose();
                self.EffectViews.Remove(fxResource);
            }
        }
    }
}