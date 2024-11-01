using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public static event Action<Plate> OnPlatePressed;

    [Header("VARIABLES")]
    [SerializeField] private List<int> plateValuesList = new List<int>();
    [Header("REFERENCES")]
    [SerializeField] private DiceSideConfigSO diceSideConfig;
    [SerializeField] private GameObject cableGO;
    [SerializeField] private Material plateColorMaterial;

    private int platePressLeft;
    private bool canBePressed = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out DiceSide side))
        {
            if(canBePressed && side.SideValue == plateValuesList[plateValuesList.Count - platePressLeft] && plateColorMaterial == diceSideConfig.CurrentDiceMaterial)
            {
                platePressLeft--;

                AudioManager.Instance.PlayPlateSFX(0.5f);

                if(platePressLeft <= 0)
                {
                    OnPlatePressed?.Invoke(this);
                    Cable cable = cableGO.GetComponent<Cable>();
                    cable.ChangeMaterial(cable.CableConfig.OnMaterial);
                    canBePressed = false;
                    return;
                }

                transform.DestroyAllChildren();
                SpawnPlate();
            }
        }
    }

    public void SpawnPlateInit()
    {
        platePressLeft = plateValuesList.Count;

        SpawnPlate();
    }
    private void SpawnPlate()
    {
        GameObject plateSideGO = Instantiate(diceSideConfig.DiceSidesGOList[plateValuesList[plateValuesList.Count - platePressLeft] - 1], transform.position, Quaternion.identity, transform);
        MeshRenderer plateSideMeshRenderer = plateSideGO.GetComponent<MeshRenderer>();
        plateSideMeshRenderer.material = plateColorMaterial;
    }

    public List<int> GetValuesList() => plateValuesList;
    public void SetValuesList(List<int> valuesList) => plateValuesList = valuesList;
    public GameObject GetCableGO() => cableGO;
    public void SetCable(GameObject cableGO) => this.cableGO = cableGO;
    public Material GetColorMaterial() => plateColorMaterial;
    public void SetColorMaterial(Material material) => plateColorMaterial = material;
}
