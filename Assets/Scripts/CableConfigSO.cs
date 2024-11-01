using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Create Cable Config")]
public class CableConfigSO : ScriptableObject
{
    public Material OffMaterial;
    public Material OnMaterial;
}
