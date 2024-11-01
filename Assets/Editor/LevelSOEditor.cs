using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSO))]
public class LevelSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelSO levelSO = (LevelSO)target;

        if(GUILayout.Button("Sort Ground Spawn Data"))
        {
            levelSO.SortGroundSpawnData();
        }
    }
}
