using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideHelper : MonoBehaviour
{
    [SerializeField] private DiceSideConfigSO diceSideConfig;
    [SerializeField] private Transform diceTransform;
    [SerializeField] private GameObject helpersGO;
    [SerializeField] private List<Transform> helperSidesTransformList = new List<Transform>(); // Forward - Left - Back - Right

    private PlayerMovement playerMovement;

    private List<int> sideValuesList = new List<int>();
    private bool hasDiceMoved = true;
    private bool hasSidesInitialized;

    private void OnEnable()
    {
        PlayerMovement.OnDiceMoved += PlayerMovement_OnDiceMoved;
    }
    private void OnDisable()
    {
        PlayerMovement.OnDiceMoved -= PlayerMovement_OnDiceMoved;
    }
    private void Start()
    {
        if(diceTransform == null) diceTransform = GameObject.FindGameObjectWithTag("Dice").transform;
        playerMovement = diceTransform.GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if(InputManager.Instance.Help)
        {
            if(!playerMovement.IsMoving)
            {
                HandleHelpInput();
            }
            else
            {
                HandleHelpRelease();
            }
        }
        else
        {
            HandleHelpRelease();
        }
    }

    private void HandleHelpInput()
    {
        if(hasDiceMoved)
        {
            InitializeHelperSides();
        }
        else
        {
            if(!helpersGO.activeInHierarchy)
            {
                helpersGO.SetActive(true);
            }
        }
    }
    private void HandleHelpRelease()
    {
        if(hasDiceMoved && hasSidesInitialized)
        {
            ClearHelperSides();
        }
        else
        {
            if(helpersGO.activeInHierarchy)
            {
                helpersGO.SetActive(false);
            }
        }
    }
    private void InitializeHelperSides()
    {
        transform.position = diceTransform.position;
        sideValuesList = GetSideValuesFromRaycasts();

        for(int i = 0; i < helperSidesTransformList.Count; i++)
        {
            GameObject diceHelperSideGO = Instantiate(diceSideConfig.DiceSidesGOList[sideValuesList[i] - 1], helperSidesTransformList[i].position, Quaternion.identity, helperSidesTransformList[i]);
            diceHelperSideGO.layer = gameObject.layer;
            MeshRenderer diceHelperSideMeshRenderer = diceHelperSideGO.GetComponent<MeshRenderer>();
            diceHelperSideMeshRenderer.material = diceSideConfig.CurrentDiceMaterial;
        }

        helpersGO.SetActive(true);

        hasDiceMoved = false;
        hasSidesInitialized = true;
    }
    private void ClearHelperSides()
    {
        foreach(Transform helperSide in helperSidesTransformList)
        {
            if(helperSide.childCount > 0)
            {
                helperSide.DestroyAllChildren();
            }
        }

        sideValuesList.Clear();

        helpersGO.SetActive(false);
        hasSidesInitialized = false;
    }
    private List<int> GetSideValuesFromRaycasts()
    {
        List<int> sideValues = new List<int>();

        bool rightSideCheck = Physics.Raycast(transform.position + Vector3.right, Vector3.left, out RaycastHit rightHit, 1f, LayerMask.GetMask("Side"));
        bool forwardSideCheck = Physics.Raycast(transform.position + Vector3.forward, Vector3.back, out RaycastHit forwardHit, 1f, LayerMask.GetMask("Side"));

        Debug.DrawRay(transform.position + Vector3.right, Vector3.left, Color.blue);
        Debug.DrawRay(transform.position + Vector3.forward, Vector3.back, Color.blue);

        int forwardDiceSideValue = forwardHit.collider.GetComponent<DiceSide>().SideValue;
        int rightDiceSideValue = rightHit.collider.GetComponent<DiceSide>().SideValue;
        int backDiceSideValue = 7 - forwardDiceSideValue;
        int leftDiceSideValue = 7 - rightDiceSideValue;

        sideValues.Add(forwardDiceSideValue);
        sideValues.Add(leftDiceSideValue);
        sideValues.Add(backDiceSideValue);
        sideValues.Add(rightDiceSideValue);

        return sideValues;
    }

    private void PlayerMovement_OnDiceMoved()
    {
        hasDiceMoved = true;
    }
}
