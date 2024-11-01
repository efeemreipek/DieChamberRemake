using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "Scriptable Objects/Create Object Type SO")]
public class ObjectTypeSO : ScriptableObject
{
    [SerializedDictionary("Object Type", "Prefab")]
    public SerializedDictionary<ObjectType, GameObject> ObjectTypeDictionary = new SerializedDictionary<ObjectType, GameObject>();

    public GameObject GetPrefab(ObjectType objectType)
    {
        if(ObjectTypeDictionary.TryGetValue(objectType, out GameObject prefab))
        {
            return prefab;
        }

        return null;
    }
}

public enum ObjectType
{
    Ground,
    Plate,
    Cable,
    Door,
    ColorChanger,
    DiceMover,
    Dice,
    DiceHelper
}
