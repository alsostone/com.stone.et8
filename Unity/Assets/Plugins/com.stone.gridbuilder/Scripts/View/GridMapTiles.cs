using UnityEngine;

namespace ST.GridBuilder
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class GridMapTiles : MonoBehaviour
    {
        [SerializeField] public GridMap gridMap;
        [SerializeField, Range(0.0f, 0.5f)] public float tileGap = 0.05f;
        
        private Vector3[] positions;
        private Color[] colors;
        private int[] indices;
        
        private void Awake()
        {
            if (gridMap == null)
                gridMap = FindObjectOfType<GridMap>();
        }
        
        public void GenerateTiles()
        {
            GridData gridData = gridMap.gridData;
            
            int len = gridData.xLength * gridData.zLength;
            if (colors == null || colors.Length != 4 * len)
                colors = new Color[4 * len];
            if (positions == null || positions.Length != 4 * len)
                positions = new Vector3[4 * len];
            if (indices == null || indices.Length != 6 * len)
                indices = new int[6 * len];
            
            int indexPosition = -1;
            int indexColor = -1;
            int indexIndices = -1;

            float size = gridData.cellSize;
            float gap = tileGap / 2.0f;
            for(int x1 = 0; x1 < gridData.xLength; x1++)
            {
                for (int z1 = 0; z1 < gridData.zLength; z1++)
                {
                    Color color = new Color(1f, 0.0f, 0.0f, 0.5f);
                    if (!gridData.GetCell(x1, z1).isObstacle)
                        color = new Color(0.0f, 1f, 0.0f, 0.5f);
                    
                    colors[++indexColor] = color;
                    colors[++indexColor] = color;
                    colors[++indexColor] = color;
                    colors[++indexColor] = color;
                    
                    positions[++indexPosition] = gridMap.RaycastPosition(new Vector3(x1 * size + gap, 0, z1 * size + gap));
                    positions[++indexPosition] = gridMap.RaycastPosition(new Vector3((x1 + 1) * size - gap, 0, z1 * size + gap));
                    positions[++indexPosition] = gridMap.RaycastPosition(new Vector3((x1 + 1) * size - gap, 0, (z1 + 1) * size - gap));
                    positions[++indexPosition] = gridMap.RaycastPosition(new Vector3(x1 * size + gap, 0, (z1 + 1) * size - gap));
                    
                    indices[++indexIndices] = indexPosition - 3;
                    indices[++indexIndices] = indexPosition - 1;
                    indices[++indexIndices] = indexPosition - 2;
                    
                    indices[++indexIndices] = indexPosition - 1;
                    indices[++indexIndices] = indexPosition - 3;
                    indices[++indexIndices] = indexPosition - 0;
                }
            }

            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.vertices = positions;
            mesh.colors = colors;
            mesh.triangles = indices;
            
            MeshFilter filter = GetComponent<MeshFilter>();
            filter.mesh = mesh;
        }
    }
}