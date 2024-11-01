using System;
using System.Collections.Generic;
using UnityEngine;


public class Screenshotter : MonoBehaviour
{
    public string FileName;
    public string FilePath;
    public int fileSuffix = 0;

    private readonly string PREFS_SUFFIX = "fileSuffix";

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Screenshot();
        }
    }

    public void Screenshot()
    {
        fileSuffix = PlayerPrefs.HasKey(PREFS_SUFFIX) ? PlayerPrefs.GetInt(PREFS_SUFFIX) : fileSuffix;

        ScreenCapture.CaptureScreenshot(FilePath + FileName + fileSuffix.ToString("D3") + ".png");

        fileSuffix++;
        PlayerPrefs.SetInt(PREFS_SUFFIX, fileSuffix);
    }
}
