using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridMapTiles : MonoBehaviour
{
    public GridMap gridMap;
    
    public void GenerateTiles()
    {
        GridData gridData = gridMap.gridData;
        
        int len = gridData.xLength * gridData.zLength;
        List<int> indices = new(6 * len);
        List<Vector3> positions = new(4 * len);
        List<Color> colors = new(4 * len);

        for(int x = 0; x < gridData.xLength; x++)
        {
            for (int z = 0; z < gridData.zLength; z++)
            {
                Color color = Color.red;
                if (gridData.CanPut(x, z))
                    color = Color.green;
                
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);

                positions.Add(gridMap.RaycastPosition(x, z));
                positions.Add(gridMap.RaycastPosition(x + 1, z));
                positions.Add(gridMap.RaycastPosition(x + 1, z + 1));
                positions.Add(gridMap.RaycastPosition(x, z + 1));

                int count = positions.Count;
                indices.Add(count - 4);
                indices.Add(count - 2);
                indices.Add(count - 3);

                indices.Add(count - 2);
                indices.Add(count - 4);
                indices.Add(count - 1);
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = positions.ToArray();
        mesh.colors = colors.ToArray();
        mesh.triangles = indices.ToArray();
        
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }
}
