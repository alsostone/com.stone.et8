using UnityEngine;

namespace ST.Mono
{
    [ExecuteInEditMode]
    public class NodeBehaviour : MonoBehaviour
    {
        public int ProxyID { get; set; }
        public bool IsDirty { get; set; }

        [SerializeField] private Vector3 previousPosition;
        [SerializeField] private Renderer nodeRenderer;
        
        private void Reset()
        {
            nodeRenderer = GetComponent<Renderer>();
            previousPosition = transform.position;
        }
        
        public Bounds GetBounds()
        {
            if (nodeRenderer == null)
                nodeRenderer = GetComponent<Renderer>();
            return nodeRenderer.bounds;
        }

        public Vector3 GetDisplacement()
        {
            return transform.position - previousPosition;
        }

        public void CleanDirty()
        {
            previousPosition = transform.position;
            IsDirty = false;
        }

        private void OnValidate()
        {
            IsDirty = true;
        }
        
        private void OnEnable()
        {
            if (DynamicTreeBehaviour.Instance)
                DynamicTreeBehaviour.Instance.Add(this);
        }

        private void OnDisable()
        {
            if (DynamicTreeBehaviour.Instance)
                DynamicTreeBehaviour.Instance.Remove(this);
        }
        
        private void Update()
        {
            if (transform.hasChanged)
            {
                IsDirty = true;
                transform.hasChanged = false;
            }
        }
        
    }
}
