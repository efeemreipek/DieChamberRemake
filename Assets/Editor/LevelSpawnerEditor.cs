using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSpawner))]
public class LevelSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelSpawner levelSpawner = (LevelSpawner)target;

        if(GUILayout.Button("Create Level"))
        {
            levelSpawner.CreateLevel();
        }
        if(GUILayout.Button("Get Level"))
        {
            levelSpawner.GetLevel();
        }
        if(GUILayout.Button("Remove Level"))
        {
            levelSpawner.RemoveLevel();
        }
        if(GUILayout.Button("Save Level"))
        {
            levelSpawner.SaveLevel();
        }
        if(GUILayout.Button("Load Level"))
        {
            levelSpawner.LoadLevel();
        }
        if(GUILayout.Button("Clear Level"))
        {
            levelSpawner.ClearLevel();
        }
        if(GUILayout.Button("Clear Dictionary"))
        {
            levelSpawner.ClearDictionary();
        }
    }
}
