using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [Serializable]
    public class AttachPointMapping
    {
        public AttachPoint name;
        public Transform transform;

        public AttachPointMapping(AttachPoint name, Transform trans)
        {
            this.name = name;
            transform = trans;
        }
    }
    
    public class AttachPointCollector : MonoBehaviour
    {
        public AttachPointMapping[] AttachPointArray;
        private Dictionary<AttachPoint, Transform> AttachPointMapping;

        private void Awake()
        {
            if (AttachPointArray != null && AttachPointArray.Length > 0)
            {
                AttachPointMapping = new Dictionary<AttachPoint, Transform>();
                foreach (var mapping in AttachPointArray) {
                    AttachPointMapping.Add(mapping.name, mapping.transform);
                }
            }
        }

        private void Reset()
        {
            AttachPointMapping = new Dictionary<AttachPoint, Transform>();
            FindMatchName(gameObject);
            
            AttachPointArray = new AttachPointMapping[AttachPointMapping.Count];
            int index = 0;
            foreach (var pair in AttachPointMapping) {
                AttachPointArray[index++] = new AttachPointMapping(pair.Key, pair.Value);
            }
        }

        private void FindMatchName(GameObject gameObj)
        {
            if (AttachPointNameMapping.Mapping.TryGetValue(gameObj.name, out var attachPoint)) {
                if (AttachPointMapping.ContainsKey(attachPoint)) {
                    Debug.LogError($"AttachPointCollector FindMatchName duplicate attach point name: {gameObj.name} in {gameObj.transform.parent.name}");
                }
                else {
                    AttachPointMapping.Add(attachPoint, gameObj.transform);
                }
            }

            for (var i = 0; i < gameObj.transform.childCount; i++) {
                FindMatchName(gameObj.transform.GetChild(i).gameObject);
            }
        }
        
        public Transform GetAttachPoint(AttachPoint attachPoint)
        {
            if (AttachPointMapping != null && AttachPointMapping.TryGetValue(attachPoint, out Transform result)) {
                return result;
            }
            return transform;
        }
    }
}

