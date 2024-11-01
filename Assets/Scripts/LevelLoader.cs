using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
	public static event Action OnLevelPassed;

    private bool isOnLoader;

    private void Awake()
    {
        isOnLoader = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Side") && !isOnLoader)
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        isOnLoader = true;
        yield return new WaitForSeconds(0.25f);
        OnLevelPassed?.Invoke();
        Debug.Log("Dice is on the level loader and invoked event");
    }
}
