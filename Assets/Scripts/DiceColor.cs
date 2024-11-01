using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceColor : MonoBehaviour
{
    [SerializeField] private DiceSideConfigSO diceSideConfig;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        diceSideConfig.CurrentDiceMaterial = diceSideConfig.DiceMaterialList[0];
    }

    public void ChangeDiceColor(Material material)
    {
        if(meshRenderer.material == material) return;

        meshRenderer.material = material;
        diceSideConfig.CurrentDiceMaterial = material;
    }
}