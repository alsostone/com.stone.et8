using System;
using UnityEngine;
namespace ST.GridBuilder
{
    public class Placement : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1.0f)] public float putHeight = 0.5f;
        [SerializeField, Range(0.1f, 1.0f)] public float takeHeight = 0.5f;
        [SerializeField, HideInInspector] public PlacementData placementData = new();
        [SerializeField] private Material previewMaterial;
        
        [NonSerialized] private Renderer[] children;
        [NonSerialized] private Material[][] originalMats;

        private void TryCacheOriginalMats()
        {
            if (children != null)
                return;
            
            children = GetComponentsInChildren<Renderer>(true);
            originalMats = new Material[children.Length][];
            for (int i = 0; i < children.Length; i++)
            {
                Renderer child = children[i];
                originalMats[i] = child.materials;
            }
        }
        
        public void SetPreviewMaterial()
        {
            if (previewMaterial != null)
            {
                TryCacheOriginalMats();
                foreach (Renderer child in children)
                {
                    Material[] newMaterials = new Material[child.materials.Length];
                    for (int i = 0; i < newMaterials.Length; i++)
                    {
                        newMaterials[i] = previewMaterial;
                    }
                    child.materials = newMaterials;
                }
            }
        }
        
        public void ResetPreviewMaterial()
        {
            if (originalMats != null)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i] != null)
                    {
                        children[i].materials = originalMats[i];
                    }
                }
            }
        }
        
        public Vector3 GetPosition()
        {
            return transform.position;
        }
        
        public void SetMovePosition(Vector3 pos)
        {
            transform.position = pos + new Vector3(0, putHeight + takeHeight, 0);
        }
        
        public void SetPutPosition(Vector3 pos)
        {
            transform.position = pos + new Vector3(0, putHeight, 0);
        }

        public void Rotation(int r)
        {
            placementData.Rotation(r);
            transform.rotation = Quaternion.Euler(0, placementData.rotation * 90, 0);
        }

        public void Remove()
        {
            DestroyImmediate(gameObject);
        }
        
        public void DoShake()
        {
            BuildingShake shake = GetComponent<BuildingShake>();
            if (shake == null) {
                shake = gameObject.AddComponent<BuildingShake>();
            }
            shake.StartShake(new Vector3(1, 0, 1));
        }
    }
}