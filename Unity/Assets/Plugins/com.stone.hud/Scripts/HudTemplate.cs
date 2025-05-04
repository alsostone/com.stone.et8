using System.Collections.Generic;
using UnityEngine;

namespace ST.HUD
{
    public class HudTemplate : MonoBehaviour
    {
        [SerializeField] private List<Vector3> vertices = new List<Vector3>();
        [SerializeField] private List<Color> colors = new List<Color>();
        [SerializeField] private List<Vector2> uvs = new List<Vector2>();
        [SerializeField] private List<int> triangles = new List<int>();

        private void Reset()
        {
            vertices.Clear();
            colors.Clear();
            uvs.Clear();
            triangles.Clear();
            GenName();
            GenProgress();
        }

        private void GenName()
        {
            var nameMesh = transform.Find("NameMesh");
            if (nameMesh == null)
                return;

            var offset = nameMesh.localPosition;
            offset.z = 0;
            var nameBox = nameMesh.localScale * 0.5f;
            
            var indexOffset = vertices.Count;
            vertices.Add(offset + new Vector3(-nameBox.x, -nameBox.y, 0));
            vertices.Add(offset + new Vector3(nameBox.x, -nameBox.y, 0));
            vertices.Add(offset + new Vector3(-nameBox.x, nameBox.y, 0));
            vertices.Add(offset + new Vector3(nameBox.x, nameBox.y, 0));

            uvs.Add(new Vector2(0f, 0f));
            uvs.Add(new Vector2(1f, 0f));
            uvs.Add(new Vector2(0f, 1f));
            uvs.Add(new Vector2(1f, 1f));

            colors.Add(Color.blue);
            colors.Add(Color.blue);
            colors.Add(Color.blue);
            colors.Add(Color.blue);
            
            triangles.Add(indexOffset + 0);
            triangles.Add(indexOffset + 2);
            triangles.Add(indexOffset + 1);
            
            triangles.Add(indexOffset + 1);
            triangles.Add(indexOffset + 2);
            triangles.Add(indexOffset + 3);
        }

        private void GenProgress()
        {
            var progressMesh = transform.Find("ProgressMesh");
            if (progressMesh == null)
                return;

            var offset = progressMesh.localPosition;
            offset.z = 0;
            var progressBox = progressMesh.localScale * 0.5f;
            
            var indexOffset = vertices.Count;
            vertices.Add(offset + new Vector3(-progressBox.x, -progressBox.y, 0));
            vertices.Add(offset + new Vector3(progressBox.x, -progressBox.y, 0));
            vertices.Add(offset + new Vector3(-progressBox.x, progressBox.y, 0));
            vertices.Add(offset + new Vector3(progressBox.x, progressBox.y, 0));

            uvs.Add(new Vector2(0f, 0f));
            uvs.Add(new Vector2(1f, 0f));
            uvs.Add(new Vector2(0f, 1f));
            uvs.Add(new Vector2(1f, 1f));

            colors.Add(Color.red + new Color(0, 0, 0, 0.1f));
            colors.Add(Color.red + new Color(0, 0, 0, 0.1f));
            colors.Add(Color.red + new Color(0, 0, 0, 0.1f));
            colors.Add(Color.red + new Color(0, 0, 0, 0.1f));
            
            triangles.Add(indexOffset + 0);
            triangles.Add(indexOffset + 2);
            triangles.Add(indexOffset + 1);
            
            triangles.Add(indexOffset + 1);
            triangles.Add(indexOffset + 2);
            triangles.Add(indexOffset + 3);
        }

        internal Mesh GenerateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.colors = colors.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.SetTriangles(triangles, 0);
            return mesh;
        }
        
    }
}