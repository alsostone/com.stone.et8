using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace ET.Client
{
    [EntitySystemOf(typeof(ResourcesPoolComponent))]
    [FriendOf(typeof(ResourcesPoolComponent))]
    public static partial class PrefabPoolComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ResourcesPoolComponent self, ResourcesLoaderComponent loader)
        {
            self.mPools = new Dictionary<string, EntityRef<ResourcesPool>>();
            self.mPoolOutObjects = new Dictionary<int, string>(200);

            self.mRoot = new GameObject().transform;
            self.mRoot.name = "ResourcesPools";
            self.mRoot.localPosition = new Vector3(0, 1000, 0);
            UnityEngine.Object.DontDestroyOnLoad(self.mRoot);
            
            self.mLoaderComponent = loader;
            if (self.LoaderComponent == null) {
                self.mLoaderComponent = self.Root().GetComponent<ResourcesLoaderComponent>();
            }
        }

        [EntitySystem]
        private static void Destroy(this ResourcesPoolComponent self)
        {
            if (self.mRoot) {
                UnityEngine.Object.Destroy(self.mRoot.gameObject);
                self.mRoot = null;
            }
        }
        
        public static GameObject Fetch(this ResourcesPoolComponent self, int tableId, Transform parent = null, bool worldPositionStays = false)
        {
            TbResourceRow res = TbResource.Instance.Get(tableId);
            return self.Fetch(res.Url, parent, worldPositionStays);
        }

        public static GameObject Fetch(this ResourcesPoolComponent self, string location, Transform parent = null, bool worldPositionStays = false)
        {
            ResourcesPool pool = null;
            if (self.mPools.TryGetValue(location, out var @ref)) {
                pool = @ref;
            } else {
                var prefab = self.LoaderComponent.LoadAssetSync<GameObject>(location);
                if (prefab == null) {
                    return null;
                }
                pool = self.AddChild<ResourcesPool, string, GameObject, Transform>(location, prefab, self.mRoot);
                self.mPools.Add(location, pool);
            }

            var go = pool.Fetch();
            if (parent != null) {
                go.transform.SetParent(parent, worldPositionStays);
            }
            self.mPoolOutObjects.Add(go.GetInstanceID(), location);
            return go;
        }

        public static ETTask<GameObject> FetchAsync(this ResourcesPoolComponent self, int tableId, Transform parent = null, bool worldPositionStays = false)
        {
            TbResourceRow res = TbResource.Instance.Get(tableId);
            return self.FetchAsync(res.Url, parent, worldPositionStays);
        }

        public static async ETTask<GameObject> FetchAsync(this ResourcesPoolComponent self, string location, Transform parent = null, bool worldPositionStays = false)
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.ResourcesPool, location.GetHashCode());

            ResourcesPool pool = null;
            if (self.mPools.TryGetValue(location, out var @ref)) {
                pool = @ref;
            } else {
                var prefab = await self.LoaderComponent.LoadAssetAsync<GameObject>(location);
                if (prefab == null) {
                    return null;
                }
                pool = self.AddChild<ResourcesPool, string, GameObject, Transform>(location, prefab, self.mRoot);
                self.mPools.Add(location, pool);
            }

            var go = pool.Fetch();
            if (parent != null) {
                go.transform.SetParent(parent, worldPositionStays);
            }
            self.mPoolOutObjects.Add(go.GetInstanceID(), location);
            return go;
        }

        public static void Preload(this ResourcesPoolComponent self, int tableId, int count)
        {
            TbResourceRow res = TbResource.Instance.Get(tableId);
            self.Preload(res.Url, count);
        }
        
        public static void Preload(this ResourcesPoolComponent self, string location, int count)
        {
            ResourcesPool pool = null;
            if (self.mPools.TryGetValue(location, out var @ref)) {
                pool = @ref;
            } else {
                var prefab = self.LoaderComponent.LoadAssetSync<GameObject>(location);
                if (prefab == null) {
                    return;
                }
                pool = self.AddChild<ResourcesPool, string, GameObject, Transform>(location, prefab, self.mRoot);
                self.mPools.Add(location, pool);
            }

            pool.EnsureIns(count);
        }

        public static ETTask PreloadAsync(this ResourcesPoolComponent self, int tableId, int count)
        {
            TbResourceRow res = TbResource.Instance.Get(tableId);
            return self.PreloadAsync(res.Url, count);
        }
        
        public static async ETTask PreloadAsync(this ResourcesPoolComponent self, string location, int count)
        {
            using CoroutineLock coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.ResourcesPool, location.GetHashCode());

            ResourcesPool pool = null;
            if (self.mPools.TryGetValue(location, out var @ref)) {
                pool = @ref;
            } else {
                var prefab = await self.LoaderComponent.LoadAssetAsync<GameObject>(location);
                if (prefab == null) {
                    return;
                }
                pool = self.AddChild<ResourcesPool, string, GameObject, Transform>(location, prefab, self.mRoot);
                self.mPools.Add(location, pool);
            }

            pool.EnsureIns(count);
        }
        
        public static void Recycle(this ResourcesPoolComponent self, GameObject obj)
        {
            var instanceId = obj.GetInstanceID();
            if(self.mPoolOutObjects.TryGetValue(instanceId, out var key))
            {
                self.mPoolOutObjects.Remove(instanceId);
                if (self.mPools.TryGetValue(key, out var @ref)) {
                    ResourcesPool pool = @ref;
#if ENABLE_DEBUG || UNITY_EDITOR
                    if (pool == null) {
                        Log.Error($"invalid gameobject pool. location:{key}");
                    }
#endif
                    pool?.Recycle(obj);
                }
            }
            else
            {
                UnityEngine.Object.Destroy(obj);
#if ENABLE_DEBUG || UNITY_EDITOR
                Log.Error($"gameobject is not create by pool. name:{obj.name}");
#endif
            }
        }

        public static void ReleasePool(this ResourcesPoolComponent self, int tableId)
        {
            TbResourceRow res = TbResource.Instance.Get(tableId);
            self.ReleasePool(res.Url);
        }
        
        public static void ReleasePool(this ResourcesPoolComponent self, string location)
        {
            if (self.mPools.TryGetValue(location, out var @ref)) {
                ResourcesPool pool = @ref;
#if ENABLE_DEBUG || UNITY_EDITOR
                if (pool == null) {
                    Log.Error($"invalid gameobject pool. location:{location}");
                }
#endif
                pool?.Dispose();
                self.mPools.Remove(location);
                self.LoaderComponent.ReleaseAsset(location);
            }
        }
        
        private static void ReleasePoolAll(this ResourcesPoolComponent self)
        {
            foreach (var item in self.mPools) {
                ResourcesPool pool = item.Value;
                pool?.Dispose();
                self.LoaderComponent.ReleaseAsset(item.Key);
            }
            self.mPools.Clear();
            self.mPoolOutObjects.Clear();
        }

        public static void ResetParticleSystem(this ResourcesPoolComponent self, GameObject go)
        {
            var instanceId = go.GetInstanceID();
            if (self.mPoolOutObjects.TryGetValue(instanceId, out var path)) {
                if (self.mPools.TryGetValue(path, out var @ref)) {
                    ResourcesPool pool = @ref;
                    pool?.ResetParticleSystem(go);
                }
            }
        }
    }

    /// <summary>
    /// 用来管理资源，生命周期跟随Parent，比如CurrentScene用到的资源应该用CurrentScene的ResourcesPoolComponent来加载
    /// 这样CurrentScene释放后，它用到的所有资源都释放了
    /// </summary>
    [ComponentOf]
    public class ResourcesPoolComponent : Entity, IAwake<ResourcesLoaderComponent>, IDestroy
    {
        public Transform mRoot;
        public Dictionary<string, EntityRef<ResourcesPool>> mPools;
        public Dictionary<int, string> mPoolOutObjects;
        
        public EntityRef<ResourcesLoaderComponent> mLoaderComponent;
        public ResourcesLoaderComponent LoaderComponent => mLoaderComponent;
    }
}