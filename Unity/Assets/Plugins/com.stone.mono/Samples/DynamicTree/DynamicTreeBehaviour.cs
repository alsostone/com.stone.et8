using System.Collections.Generic;
using UnityEngine;

namespace ST.Mono
{
    [ExecuteInEditMode]
    public class DynamicTreeBehaviour : MonoBehaviour
    {
        private static DynamicTreeBehaviour sInstance;
        public static DynamicTreeBehaviour Instance => sInstance ? sInstance : sInstance = FindObjectOfType<DynamicTreeBehaviour>();

        [SerializeField] private int spawnCount = 50;
        [SerializeField] private Vector3 spawnSize = new Vector3(50f, 50f, 50f);
        [SerializeField] private Color[] depthColors;

        private DynamicTree<NodeBehaviour> dynamicTree;
        private readonly List<NodeBehaviour> gameObjects = new List<NodeBehaviour>(32);
        
        private void Reset()
        {
            depthColors = new Color[]
            {
                Color.red,
                Color.green,
                Color.blue,
                Color.cyan,
                Color.magenta,
                Color.yellow
            };
            dynamicTree = null;
        }
        
        public void SpawnNodeObject()
        {
            Transform parent = new GameObject("Objects").transform;
            for (int i = 0; i < spawnCount; i++)
            {
                GameObject cubeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubeObject.name = "Cube";
                cubeObject.transform.SetParent(parent);
                cubeObject.transform.position = new Vector3(Random.value * spawnSize.x, Random.Range(0, 3) * 10, Random.value * spawnSize.z);
                cubeObject.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
                cubeObject.transform.localScale = new Vector3(Random.value * 5, Random.value * 5, Random.value * 5);
                cubeObject.AddComponent<NodeBehaviour>();
            }
        }
        
        public void RebuildBottomUp()
        {
            EnsureInitialized();
            dynamicTree.RebuildBottomUp();
        }

        private void EnsureInitialized()
        {
            if (dynamicTree == null) {
                dynamicTree = new DynamicTree<NodeBehaviour>(32);
                gameObjects.Clear();
                NodeBehaviour[] objs = FindObjectsOfType<NodeBehaviour>();
                for (int i = 0; i < objs.Length; i++)
                {
                    Add(objs[i]);
                }
            }
        }

        private void Update()
        {
            EnsureInitialized();
            foreach (NodeBehaviour obj in gameObjects)
            {
                if (obj.IsDirty)
                {
                    dynamicTree.MoveProxy(obj.ProxyID, obj.GetBounds().ToAABB(), obj.GetDisplacement().ToTSVector());
                    obj.CleanDirty();
                }
            }
        }

        public void Add(NodeBehaviour obj)
        {
            EnsureInitialized();
            if (gameObjects.Contains(obj))
                return;
            
            gameObjects.Add(obj);
            obj.ProxyID = dynamicTree.CreateProxy(obj.GetBounds().ToAABB(), obj);
        }

        public void Remove(NodeBehaviour obj)
        {
            EnsureInitialized();
            if (!gameObjects.Contains(obj))
                return;
            
            gameObjects.Remove(obj);
            dynamicTree.DestroyProxy(obj.ProxyID);
        }

        public void OnDrawGizmos()
        {
            if (dynamicTree == null)
                return;

            foreach ((DynamicTree<NodeBehaviour>.TreeNode node, int depth) in dynamicTree.EnumerateNodes())
            {
                int nodeDepth = depth % depthColors.Length;
                Gizmos.color = depthColors[nodeDepth];
                Gizmos.DrawWireCube(node.AABB.Center.ToVector3(), node.AABB.Size.ToVector3());
            }
        }
    }


}
