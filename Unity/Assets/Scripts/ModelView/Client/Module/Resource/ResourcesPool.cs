using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(ResourcesPool))]
    [FriendOf(typeof(ResourcesPool))]
    public static partial class ResourcesPoolSystem
    {
        [EntitySystem]
        private static void Awake(this ResourcesPool self, string location, GameObject prefab, Transform rootParent)
        {
            self.mQueue = new Stack<GameObject>();
            self.mOccupy = new HashSet<GameObject>();
            self.mLocation = location;
            self.mPrefab = prefab;

            self.mRoot = new GameObject().transform;
            self.mRoot.name = location;
            self.mRoot.SetParent(rootParent);
            self.mRoot.localPosition = Vector3.zero;

            self.mCacheParticleSystem = new Dictionary<int, List<ParticleSystem>>(4);
        }

        [EntitySystem]
        private static void Destroy(this ResourcesPool self)
        {
            foreach (var obj in self.mQueue) {
                UnityEngine.Object.Destroy(obj);
            }
#if ENABLE_DEBUG || UNITY_EDITOR
            if (self.mOccupy.Count > 0) {
                Log.Error($"there has gameobject not recycle. location:{self.mLocation}, count:{self.mOccupy.Count}");
            }
#endif
            if (self.mRoot != null) {
                UnityEngine.Object.Destroy(self.mRoot.gameObject);
                self.mRoot = null;
            }
        }
        
        public static void EnsureIns(this ResourcesPool self, int count)
        {
            for (var i = self.mQueue.Count; i < count; i++) {
                self.mQueue.Push(UnityEngine.Object.Instantiate(self.mPrefab, self.mRoot, false));
            }
        }
        
        public static GameObject Fetch(this ResourcesPool self) 
        {
            GameObject go = null;
            if (self.mQueue.Count == 0) {
                go = UnityEngine.Object.Instantiate(self.mPrefab);
            } else {
                go = self.mQueue.Pop();
            }
            if (go != null) {
                self.mOccupy.Add(go);
            }
            return go;
        }
        
        public static void Recycle(this ResourcesPool self, GameObject obj)
        {
            self.mQueue.Push(obj);
            if (self.mOccupy.Contains(obj)) {
                self.mOccupy.Remove(obj);
            }
            
            obj.transform.SetParent(self.mRoot, false);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = self.mPrefab.transform.localRotation;
            obj.transform.localScale = self.mPrefab.transform.localScale;
            
            // reset rect transform
            var objRect = obj.transform.GetComponent<RectTransform>();
            var mPrefabRect = self.mPrefab.transform.GetComponent<RectTransform>();
            if (objRect != null && mPrefabRect != null) {
                objRect.pivot = mPrefabRect.pivot;
                objRect.anchorMax = mPrefabRect.anchorMax;
                objRect.anchorMin = mPrefabRect.anchorMin;
            }
        }

        public static void ResetParticleSystem(this ResourcesPool self, GameObject go)
        {
            var instanceId = go.GetInstanceID();
            if (!self.mCacheParticleSystem.TryGetValue(instanceId, out var particleSystems)) {
                self.mCacheParticleSystem.Add(instanceId, new List<ParticleSystem>());
                go.GetComponentsInChildren(true, self.mCacheParticleSystem[instanceId]);
                particleSystems = self.mCacheParticleSystem[instanceId];
            }
            foreach (var item in particleSystems)
            {
                item.Clear(false);
                item.Play();
            }
        }
    }
    
    [ChildOf(typeof(ResourcesPoolComponent))]
    public class ResourcesPool : Entity, IAwake<string, GameObject, Transform>, IDestroy
    {
        public string mLocation;
        public HashSet<GameObject> mOccupy;
        public Stack<GameObject> mQueue;
        public GameObject mPrefab;
        public Transform mRoot;
        public Dictionary<int, List<ParticleSystem>> mCacheParticleSystem;
    }
}