using System;
using UnityEngine;

namespace ET.Client
{
    public static class GameObjectExtends
    {
        public static GameObject SetActiveGracefully(this GameObject selfObj, bool value)
        {
            if (selfObj.activeSelf != value)
                selfObj.SetActive(value);
            return selfObj;
        }
        public static T SetActiveGracefully<T>(this T selfComponent, bool value) where T : Component
        {
            if (selfComponent.gameObject.activeSelf != value)
                selfComponent.gameObject.SetActive(value);
            return selfComponent;
        }
        public static void DestroyGameObj<T>(this T selfBehaviour) where T : Component
        {
            if (selfBehaviour.gameObject)
                UnityEngine.Object.Destroy(selfBehaviour.gameObject);
        }
        public static T DestroyGameObjDelay<T>(this T selfBehaviour, float delay) where T : Component
        {
            if (selfBehaviour.gameObject)
                UnityEngine.Object.Destroy(selfBehaviour.gameObject, delay);
            return selfBehaviour;
        }
        public static GameObject Layer(this GameObject selfObj, int layer)
        {
            selfObj.layer = layer;
            return selfObj;
        }
        public static T Layer<T>(this T selfComponent, int layer) where T : Component
        {
            selfComponent.gameObject.layer = layer;
            return selfComponent;
        }
        public static GameObject Layer(this GameObject selfObj, string layerName)
        {
            selfObj.layer = LayerMask.NameToLayer(layerName);
            return selfObj;
        }
        public static T Layer<T>(this T selfComponent, string layerName) where T : Component
        {
            selfComponent.gameObject.layer = LayerMask.NameToLayer(layerName);
            return selfComponent;
        }
        public static T AddMissingComponent<T>(this GameObject selfObj) where T : Component
        {
            var comp = selfObj.GetComponent<T>();
            return comp != null ? comp : selfObj.AddComponent<T>();
        }
        public static Component AddMissingComponent(this GameObject selfObj, Type type)
        {
            var comp = selfObj.GetComponent(type);
            return comp ? comp : selfObj.AddComponent(type);
        }
        public static void SetObjectColor(this GameObject selfObj, string propertyName, Color color)
        {
            Material[] mats = selfObj.GetComponent<Renderer>().materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].SetColor(propertyName, color);
            }
        }
        public static void SetObjectColor(this GameObject selfObj, Color color)
        {
            Material[] mats = selfObj.GetComponent<Renderer>().materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i].color = color;
            }
        }

    }
}