using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ConnectedMeshGenerator : MonoBehaviour
{
    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();

    public void GenerateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices.Clear();
        triangles.Clear();

        Transform[] children = GetComponentsInChildren<Transform>();

        if(children.Length < 2)
        {
            Debug.LogError("This object must have at least two child objects to form a mesh.");
            return;
        }

        Vector3 previousEnd = Vector3.zero;
        bool isFirstQuad = true;

        for(int i = 1; i < children.Length - 1; i++)
        {
            Transform startPoint = children[i];
            Transform endPoint = children[i + 1];

            Vector3 startScale = startPoint.localScale;
            Vector3 endScale = endPoint.localScale;

            // Offset the start by half the scale of the previous quad
            Vector3 start = startPoint.localPosition;
            if(!isFirstQuad)
            {
                // Adjust the start position to be back by half the previous quad's width
                start = startPoint.localPosition - (endPoint.localPosition - startPoint.localPosition).normalized * startScale.x * 0.5f;
            }

            Vector3[] quadVertices = CreateQuad(start, endPoint.localPosition, startScale, endScale);
            AddQuadToMesh(quadVertices);

            previousEnd = endPoint.localPosition;
            isFirstQuad = false;
        }

        UpdateMesh();
    }

    private Vector3[] CreateQuad(Vector3 start, Vector3 end, Vector3 startScale, Vector3 endScale)
    {
        Vector3 direction = (end - start).normalized;

        // Calculate orthogonal vector for width
        Vector3 orthogonal = Vector3.Cross(direction, Vector3.up).normalized;

        Vector3[] quadVertices = new Vector3[4];

        // Adjust the quad by half the width of the scale
        quadVertices[0] = start - orthogonal * startScale.x * 0.5f;
        quadVertices[1] = start + orthogonal * startScale.x * 0.5f;
        quadVertices[2] = end - orthogonal * endScale.x * 0.5f;
        quadVertices[3] = end + orthogonal * endScale.x * 0.5f;

        return quadVertices;
    }

    private void AddQuadToMesh(Vector3[] quadVertices)
    {
        int startIndex = vertices.Count;

        vertices.AddRange(quadVertices);

        triangles.Add(startIndex + 0);
        triangles.Add(startIndex + 1);
        triangles.Add(startIndex + 2);

        triangles.Add(startIndex + 1);
        triangles.Add(startIndex + 3);
        triangles.Add(startIndex + 2);
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    public void GenerateCable()
    {
        GenerateMesh();
        UpdateMesh();
    }
}
