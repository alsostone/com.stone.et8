using System.Collections.Generic;
using ET;
using ET.Client;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIFramework
{
    public class YIUIListView<TItemRenderer> where TItemRenderer : Entity, IYIUIBind, IYIUIInitialize
    {
        private readonly Entity m_OwnerEntity;
        private readonly YIUIBindVo m_BindVo;
        private readonly Transform m_Owner;
        private readonly ObjCache<TItemRenderer> m_UIBasePool;
        
        private RectTransform CacheRect;
        public readonly List<TItemRenderer> ItemRenderers = new ();

        public YIUIListView(Entity ownerEneity, Transform owner)
        {
            var data = YIUIBindHelper.GetBindVoByType<TItemRenderer>();
            if (data == null) return;
            m_BindVo = data.Value;
            m_UIBasePool = new ObjCache<TItemRenderer>(OnCreateItemRenderer);
            m_OwnerEntity = ownerEneity;
            m_Owner = owner;
            InitClearContent();
            InitCacheParent();
        }

        //不应该初始化时有内容 所有不管是什么全部摧毁
        private void InitClearContent()
        {
            var count = m_Owner.childCount;
            for (var i = count - 1; i >= 0; i--)
            {
                var child = m_Owner.GetChild(i);
                UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
        }

        private void InitCacheParent()
        {
            var cacheObj = new GameObject("Cache");
            CacheRect = cacheObj.GetOrAddComponent<RectTransform>();
            CacheRect.SetParent(m_Owner.parent, false);
            cacheObj.SetActive(false);
        }

        private TItemRenderer OnCreateItemRenderer()
        {
            var uiBase = YIUIFactory.Instantiate<TItemRenderer>(m_BindVo, m_OwnerEntity);
            return uiBase;
        }

        public TItemRenderer CreateItemRenderer()
        {
            var renderer = m_UIBasePool.Get();
            renderer.GetParent<YIUIComponent>().OwnerRectTransform.SetParent(m_Owner, false);
            ItemRenderers.Add(renderer);
            return renderer;
        }

        public void RemoveItemRenderer(int index)
        {
            if (index < 0 || index >= ItemRenderers.Count)
                return;
            var renderer = ItemRenderers[index];
            ItemRenderers.RemoveAt(index);
            m_UIBasePool.Put(renderer);
            renderer.GetParent<YIUIComponent>().OwnerRectTransform.SetParent(CacheRect, false);
        }

        public void Clear()
        {
            for (var i = ItemRenderers.Count - 1; i >= 0; i--)
            {
                var renderer = ItemRenderers[i];
                m_UIBasePool.Put(renderer);
                renderer.GetParent<YIUIComponent>().OwnerRectTransform.SetParent(CacheRect, false);
            }
            ItemRenderers.Clear();
        }
        
    }
}