using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class Door : MonoBehaviour
{ 
    [SerializeField] private List<Transform> doorsTransformList = new List<Transform>();

    private void OnEnable()
    {
        PlateController.OnAllPlatesPressed += PlateController_OnAllPlatesPressed;
    }

    private void OnDisable()
    {
        PlateController.OnAllPlatesPressed -= PlateController_OnAllPlatesPressed;
    }

    private void PlateController_OnAllPlatesPressed()
    {
        foreach (Transform child in doorsTransformList)
        {
            Tween.ScaleX(child, endValue: 0.05f, duration: 1f);
        }

        AudioManager.Instance.PlayDoorSFX(0.5f, true, 0.15f);
    }
}
