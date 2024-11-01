using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceColorChanger : MonoBehaviour
{
    [SerializeField] private DiceSideConfigSO diceSideConfig;
    [SerializeField] private Material colorToChange; 

    private void Update()
    {
        bool diceCheck = Physics.Raycast(transform.position + Vector3.up * 1.5f, Vector3.down, out RaycastHit dicehit, 1.5f, LayerMask.GetMask("Side"));
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, Vector3.down * 1.5f, diceCheck ? Color.blue : Color.magenta);

        if(diceCheck)
        {
            dicehit.collider.GetComponentInParent<DiceColor>().ChangeDiceColor(colorToChange);
        }
    }

    public Material GetColorMaterial() => colorToChange;
    public void SetColorMaterial(Material color) => colorToChange = color;
}
