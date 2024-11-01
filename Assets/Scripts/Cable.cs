using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public CableConfigSO CableConfig;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        ChangeMaterial(CableConfig.OffMaterial);
    }

    public void ChangeMaterial(Material mat)
    {
        if(meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = mat;
    }
}
