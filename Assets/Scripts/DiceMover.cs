using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class DiceMover : MonoBehaviour
{
    [SerializeField] private float moveTime = 1f;
    [SerializeField] private Transform middleTransform;
    [SerializeField] private Transform endTransform;

    private void Update()
    {
        bool diceHit = Physics.Raycast(transform.position, transform.right, out RaycastHit hit, 1f, LayerMask.GetMask("Side"));
        Debug.DrawRay(transform.position, transform.right, diceHit ? Color.yellow : Color.black);

        if(diceHit)
        {
            PlayerMovement diceMovement = hit.collider.GetComponentInParent<PlayerMovement>();
            Transform diceTransform = diceMovement.transform;

            Sequence.Create()
                .Group(Tween.LocalPositionX(endTransform, 1.375f, moveTime * 1.5f))
                .Group(Tween.ScaleX(middleTransform, 1.25f, moveTime * 1.5f))
                .Group(Tween.LocalPositionX(middleTransform, 0.625f, moveTime * 1.5f)).OnComplete(target: this, target => target.MoveBack());

            diceMovement.MoveSideways(transform.right, moveTime);
        }

    }

    private void MoveBack()
    {
        Sequence.Create()
            .Group(Tween.LocalPositionX(endTransform, 0.275f, moveTime * 2f))
            .Group(Tween.ScaleX(middleTransform, 0.25f, moveTime * 2f))
            .Group(Tween.LocalPositionX(middleTransform, 0.125f, moveTime * 2f));
    }
}
