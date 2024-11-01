using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Screenshotter))]
public class ScreenshotterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Screenshotter screenshotter = (Screenshotter)target;

        if(GUILayout.Button("Take Screenshot"))
        {
            screenshotter.Screenshot();
        }
    }
}

