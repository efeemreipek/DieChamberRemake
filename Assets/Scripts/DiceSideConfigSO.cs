using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Create Plate Config")]
public class DiceSideConfigSO : ScriptableObject
{
    public List<GameObject> DiceSidesGOList = new List<GameObject>();
    public List<Material> DiceMaterialList = new List<Material>();
    public Material CurrentDiceMaterial;
}
