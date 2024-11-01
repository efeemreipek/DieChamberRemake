using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTypeGetter : MonoBehaviour
{
	[SerializeField] private ObjectType objectType;

    public ObjectType GetObjectType() => objectType;
}
