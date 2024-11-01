using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    public static event Action OnAllPlatesPressed;

    private List<Plate> platesList = new List<Plate>();
    private List<Plate> platesLeftList = new List<Plate>();

    private bool areAllPlatesPressed;

    private void OnEnable()
    {
        GameObject[] platesGO = GameObject.FindGameObjectsWithTag("Plate");

        foreach (GameObject plate in platesGO)
        {
            platesList.Add(plate.GetComponent<Plate>());
        }

        platesLeftList = platesList;

        Plate.OnPlatePressed += Plate_OnPlatePressed;
    }

    private void OnDisable()
    {
        platesList.Clear();
        platesLeftList = platesList;

        Plate.OnPlatePressed -= Plate_OnPlatePressed;
    }

    private void Update()
    {
        if(platesLeftList.Count == 0 && !areAllPlatesPressed)
        {
            OnAllPlatesPressed?.Invoke();
            areAllPlatesPressed = true;
        }
    }

    private void Plate_OnPlatePressed(Plate plate)
    {
        platesLeftList.Remove(plate);
    }
}
