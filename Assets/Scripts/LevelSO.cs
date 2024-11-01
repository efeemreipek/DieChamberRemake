using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Create New Level")]
public class LevelSO : ScriptableObject
{
	public int LevelMoves;
	[TextArea(2,5)] public string LevelInstruction;
	public Vector3 CameraPosition;
	public List<SpawnData> SpawnDataList = new List<SpawnData>();

	public void SortGroundSpawnData()
	{
		var sortedGroundSpawnData = SpawnDataList.Where(data => data.ObjectType == ObjectType.Ground)
												 .OrderBy(data => data.Position.z)
												 .ThenBy(data =>  data.Position.x)
												 .ToList();

		SpawnDataList.RemoveAll(data => data.ObjectType == ObjectType.Ground);
		SpawnDataList.InsertRange(0, sortedGroundSpawnData);
	}
}

[System.Serializable]
public class SpawnData
{
	public ObjectType ObjectType;
	public Vector3 Position;
	public Quaternion Rotation;
	public Vector3 Scale;
	public List<CablePoint> CablePoints;
	public PlateReferences PlateReferences;
	public Material ColorChangerMaterial;

	public SpawnData(ObjectType type, Vector3 position, Quaternion rotation = default, Vector3 scale = default)
	{
		ObjectType = type;
		Position = position;
		Rotation = rotation == default ? Quaternion.identity : rotation;
		Scale = scale == default ? Vector3.one : scale;

		if(type == ObjectType.Cable)
		{
			CablePoints = new List<CablePoint>();
		}
	}
}

[System.Serializable]
public class CablePoint
{
	public Vector3 Position;
	public Vector3 Scale;

	public CablePoint(Vector3 position, Vector3 scale)
    {
        Position = position;
        Scale = scale;
    }
}

[System.Serializable]
public class PlateReferences
{
	public List<int> ValuesList;
	public GameObject ConnectedCableGO;
	public Material ColorMaterial;

	public PlateReferences(List<int> valuesList, GameObject connectedCableGO, Material colorMaterial)
    {
        ValuesList = valuesList;
        ConnectedCableGO = connectedCableGO;
        ColorMaterial = colorMaterial;
    }
}
