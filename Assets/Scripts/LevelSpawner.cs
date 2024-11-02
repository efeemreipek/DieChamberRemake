using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR
using System;

public class LevelSpawner : MonoBehaviour
{
    public static event Action OnLevelRestarted;
    public static event Action OnGameEnded;
    public static event Action<LevelSO> OnLevelLoaded;

    [SerializeField] private ObjectTypeSO objectTypeSO;
    [SerializeField] private Transform levelCreatorTransform;
	[SerializeField] private List<LevelSO> levelList = new List<LevelSO>();
	[SerializeField] private LevelSO currentLevel;
	[SerializeField] private int levelToLoadIndex;
	[SerializeField] private int levelToCreateIndex;

    [SerializedDictionary("Index", "LevelGO")]
	public SerializedDictionary<int, GameObject> levelGODictionary = new SerializedDictionary<int, GameObject>();

    private int currentLevelIndex;

    private void OnEnable()
    {
        LevelLoader.OnLevelPassed += LevelLoader_OnLevelPassed;
        LevelRestarter.OnLevelRestarted += LevelRestarter_OnLevelRestarted;
        UIManager.OnGameStarted += UIManager_OnGameStarted; 
        UIManager.OnPlayAgain += UIManager_OnPlayAgain;
    }

    private void UIManager_OnGameStarted()
    {
        LoadLevel(0);
    }

    private void LevelRestarter_OnLevelRestarted()
    {
        Debug.Log("In Level Spawner to restart");
        StartCoroutine(ClearLevelForRestart());
    }

    private void UIManager_OnPlayAgain()
    {
        OnLevelRestarted?.Invoke();
        LoadLevel(currentLevelIndex);
    }

    private void LevelLoader_OnLevelPassed()
    {
        StartCoroutine(ClearLevelForNextLevel());
    }

    public void CreateLevel()
    {
#if UNITY_EDITOR
        LevelSO newLevel = ScriptableObject.CreateInstance<LevelSO>();
        AssetDatabase.CreateAsset(newLevel, $"Assets/Scriptable Objects/Level SO/Level{levelToCreateIndex}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        levelList.Add(newLevel);
        LoadLevel(levelToCreateIndex);

        Debug.Log(AssetDatabase.GetAssetPath(newLevel));
#endif // UNITY_EDITOR
    }

    public void GetLevel()
    {
        currentLevel = levelList[levelToLoadIndex];
    }

    public void RemoveLevel()
    {
        currentLevel = null;
    }

    public void SaveLevel()
    {
        currentLevel.SpawnDataList.Clear();

        GameObject lastObject = null;

        foreach(Transform child in levelCreatorTransform)
        {
            ObjectType type = child.GetComponent<ObjectTypeGetter>().GetObjectType();
            SpawnData spawnData = new SpawnData(type, child.position, child.rotation, child.localScale);

            if(type == ObjectType.Cable)
            {
                foreach(Transform cablePoint in child)
                {
                    spawnData.CablePoints.Add(new CablePoint(cablePoint.localPosition, cablePoint.localScale));
                }
            }
            if(type == ObjectType.Plate)
            {
                Plate plate = child.GetComponent<Plate>();
                spawnData.PlateReferences = new PlateReferences(plate.GetValuesList(), lastObject, plate.GetColorMaterial());
            }
            if(type == ObjectType.ColorChanger)
            {
                spawnData.ColorChangerMaterial = child.GetComponent<DiceColorChanger>().GetColorMaterial();
            }

            currentLevel.SpawnDataList.Add(spawnData);

            lastObject = child.gameObject;
        }

        Debug.Log($"{currentLevel} is saved");
    }

    public void LoadLevel()
    {
        LoadLevel(levelToLoadIndex);
    }

    private void LoadLevel(int index)
    {
        Debug.Log("In LoadLevel function");

        if(currentLevel != null) ClearLevel();

        currentLevel = levelList[index];
        currentLevelIndex = index;

        if(!levelGODictionary.TryGetValue(index, out GameObject levelGO))
        {
            levelGO = new GameObject($"Level {index}");
            levelGO.transform.SetParent(this.transform);
            levelGODictionary.Add(index, levelGO);
        }

        Camera.main.transform.position = currentLevel.CameraPosition;

        StartCoroutine(SpawnObjects(levelGO, currentLevel.SpawnDataList));

        //OnLevelLoaded?.Invoke(currentLevel);
        Debug.Log($"{currentLevel} is loaded");
    }

    private void LoadNextLevel()
    {
        if(currentLevelIndex == levelList.Count - 1)
        {
            Debug.Log("This is the last level");
            OnGameEnded?.Invoke();
            return;
        }

        LoadLevel(++currentLevelIndex);
    }

    private IEnumerator ClearLevelForNextLevel()
    {
        yield return ClearLevelForLoad();

        LoadNextLevel();
    }

    private IEnumerator ClearLevelForRestart()
    {
        yield return ClearLevelForLoad();

        LoadLevel(currentLevelIndex);
    }

    private IEnumerator ClearLevelForLoad()
    {
        foreach(Transform c in levelGODictionary[currentLevelIndex].transform)
        {
            if(c.TryGetComponent(out ObjectSpawnAnimator spawnAnimator))
            {
                spawnAnimator.StartCoroutineScaleOut();
                yield return null;
            }
        }
        Debug.Log("Level cleared for next level");

        yield return new WaitForSeconds(ObjectSpawnAnimatorParameters.Instance.AnimationTimeOut);

        Debug.Log("Waited before loading next level");
    }

    private IEnumerator SpawnObjects(GameObject parent, List<SpawnData> spawnDataList)
    {
        GameObject lastSpawnedObject = null;

        foreach(SpawnData spawnData in spawnDataList)
        {
            GameObject obj = Instantiate(objectTypeSO.GetPrefab(spawnData.ObjectType), spawnData.Position, spawnData.Rotation, parent.transform);
            yield return null;

            if(spawnData.ObjectType == ObjectType.Cable)
            {
                foreach(CablePoint cablePoint in spawnData.CablePoints)
                {
                    GameObject cableChild = new GameObject("CablePoint");
                    cableChild.transform.SetParent(obj.transform);
                    cableChild.transform.localPosition = cablePoint.Position;
                    cableChild.transform.localScale = cablePoint.Scale;
                }

                obj.GetComponent<ConnectedMeshGenerator>().GenerateCable();
            }
            if(spawnData.ObjectType == ObjectType.Plate)
            {
                Plate plate = obj.GetComponent<Plate>();
                plate.SetValuesList(spawnData.PlateReferences.ValuesList);
                plate.SetCable(lastSpawnedObject); // Cables must be spawned just before plates
                plate.SetColorMaterial(spawnData.PlateReferences.ColorMaterial);

                plate.SpawnPlateInit();
            }
            if(spawnData.ObjectType == ObjectType.Dice)
            {
                PlayerMovement playerMovement = obj.GetComponent<PlayerMovement>();
                playerMovement.ChangeMoveAmount(currentLevel.LevelMoves);
            }
            if(spawnData.ObjectType == ObjectType.ColorChanger)
            {
                obj.GetComponent<DiceColorChanger>().SetColorMaterial(spawnData.ColorChangerMaterial);
            }

            lastSpawnedObject = obj;
        }

        GameObject plateControllerGO = new GameObject("Plate Controller");
        plateControllerGO.transform.localPosition = Vector3.zero; 
        plateControllerGO.transform.SetParent(parent.transform);
        plateControllerGO.AddComponent<PlateController>();


        OnLevelLoaded?.Invoke(currentLevel);
    }

    public void ClearLevel()
	{
        foreach(Transform child in transform)
        {

#if UNITY_EDITOR
            child.DestroyAllChildrenImmediate();
#else
			child.DestroyAllChildren();
#endif
        }

        currentLevel = null;
    }

    public void ClearDictionary()
    {

#if UNITY_EDITOR
        transform.DestroyAllChildrenImmediate();
#else
		transform.DestroyAllChildren();
#endif

        levelGODictionary.Clear();
    }

}
