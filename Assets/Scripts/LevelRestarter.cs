using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRestarter : MonoBehaviour
{
	public static event Action OnLevelRestarted;
    public static event Action<float> OnLevelRestartProgress;

	private float restartTimer = 1f;
	private float restartTimerCount = 0f;
    private bool canRestart = true;
    private bool isEventFired = false;

    private void Update()
    {
        if(InputManager.Instance.Restart && canRestart && !isEventFired)
        {
            restartTimerCount += Time.deltaTime;
            OnLevelRestartProgress?.Invoke(restartTimerCount);

            if(restartTimerCount >= restartTimer)
            {
                restartTimerCount = 0f;
                OnLevelRestartProgress?.Invoke(restartTimerCount);
                canRestart = false;
                isEventFired = true;
                OnLevelRestarted?.Invoke();
            }
        }
        else
        {
            if(!InputManager.Instance.Restart)
            {
                restartTimerCount = 0f;
                OnLevelRestartProgress?.Invoke(restartTimerCount);
                canRestart = true;
                isEventFired = false;
            }
        }
    }
}
