using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConnectedMeshGenerator))]
public class ConnectedMeshGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ConnectedMeshGenerator meshGenerator = (ConnectedMeshGenerator)target;

        if(GUILayout.Button("Generate Mesh"))
        {
            meshGenerator.GenerateMesh();
        }
    }
}

