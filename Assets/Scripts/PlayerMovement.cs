using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class PlayerMovement : MonoBehaviour
{
    public static event Action OnDiceMoved;
    public static event Action<int> OnMoveAmountChanged;
    public static event Action OnMoveAmountZero;

    [Header("VARIABLES")]
    [SerializeField] private float rollSpeed;
    [SerializeField] private float rollWait;
    [SerializeField] private float rayLength = 2f;
    [SerializeField] private int moveAmount;

    private WaitForSeconds rollWaitSeconds;
    
    private bool isMoving;
    private bool isGameOver;
    private int moveAmountLeft = 1;

    public bool IsMoving {  get { return isMoving; }
                            set { isMoving = value; } }

    private void Awake()
    {
        rollWaitSeconds = new WaitForSeconds(rollWait);
    }

    private void Update()
    {
        if(isGameOver) return;
        if(isMoving) return;
        if(InputManager.Instance.Help) return;
        if(CheckMoveAmount()) return;

        bool forwardCheck = Physics.Raycast(transform.position, Vector3.forward, rayLength, LayerMask.GetMask("Obstacle"));
        bool backCheck = Physics.Raycast(transform.position, Vector3.back, rayLength, LayerMask.GetMask("Obstacle"));
        bool rightCheck = Physics.Raycast(transform.position, Vector3.right, rayLength, LayerMask.GetMask("Obstacle"));
        bool leftCheck = Physics.Raycast(transform.position, Vector3.left, rayLength, LayerMask.GetMask("Obstacle"));

        Debug.DrawRay(transform.position, Vector3.forward * rayLength, forwardCheck ? Color.red : Color.green);
        Debug.DrawRay(transform.position, Vector3.back * rayLength, backCheck ? Color.red : Color.green);
        Debug.DrawRay(transform.position, Vector3.right * rayLength, rightCheck ? Color.red : Color.green);
        Debug.DrawRay(transform.position, Vector3.left * rayLength, leftCheck ? Color.red : Color.green);

        bool forwardDownCheck = false, backDownCheck = false, rightDownCheck = false, leftDownCheck = false;

        if(!forwardCheck)
        {
            forwardDownCheck = Physics.Raycast(transform.position + Vector3.forward * rayLength, Vector3.down, rayLength, LayerMask.GetMask("Obstacle"));
            Debug.DrawRay(transform.position + Vector3.forward * rayLength, Vector3.down * rayLength, forwardDownCheck ? Color.red : Color.green);
        }
        if(!backCheck)
        {
            backDownCheck = Physics.Raycast(transform.position + Vector3.back * rayLength, Vector3.down, rayLength, LayerMask.GetMask("Obstacle"));
            Debug.DrawRay(transform.position + Vector3.back * rayLength, Vector3.down * rayLength, backDownCheck ? Color.red : Color.green);
        }
        if(!rightCheck)
        {
            rightDownCheck = Physics.Raycast(transform.position + Vector3.right * rayLength, Vector3.down, rayLength, LayerMask.GetMask("Obstacle"));
            Debug.DrawRay(transform.position + Vector3.right * rayLength, Vector3.down * rayLength, rightDownCheck ? Color.red : Color.green);
        }
        if(!leftCheck)
        {
            leftDownCheck = Physics.Raycast(transform.position + Vector3.left * rayLength, Vector3.down, rayLength, LayerMask.GetMask("Obstacle"));
            Debug.DrawRay(transform.position + Vector3.left * rayLength, Vector3.down * rayLength, leftDownCheck ? Color.red : Color.green);
        }

        if(InputManager.Instance.MoveRight && !rightCheck && rightDownCheck)
        {
            Assemble(Vector3.right);
        }
        else if(InputManager.Instance.MoveLeft && !leftCheck && leftDownCheck)
        {
            Assemble(Vector3.left);
        }
        else if(InputManager.Instance.MoveForward && !forwardCheck && forwardDownCheck)
        {
            Assemble(Vector3.forward);
        }
        else if(InputManager.Instance.MoveBack && !backCheck && backDownCheck)
        {
            Assemble(Vector3.back);
        }
    }

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        isMoving = true;
        OnDiceMoved?.Invoke();

        moveAmountLeft--;
        OnMoveAmountChanged?.Invoke(moveAmountLeft);

        AudioManager.Instance.PlayDiceMoveSFX(0.15f);

        for(int i = 0; i < (90 / rollSpeed); i++)
        {
            transform.RotateAround(anchor, axis, rollSpeed);
            yield return rollWaitSeconds;
        }

        yield return null;
        isMoving = false;
    }

    private void Assemble(Vector3 dir)
    {
        Vector3 anchor = transform.position + (Vector3.down + dir) * 0.5f;
        Vector3 axis = Vector3.Cross(Vector3.up, dir);
        StartCoroutine(Roll(anchor, axis));
    }

    private bool CheckMoveAmount()
    {
        if(moveAmountLeft <= 0)
        {
            moveAmountLeft = 0;
            OnMoveAmountZero?.Invoke();
            isGameOver = true;
            return true;
        }

        return false;
    }

    public void MoveSideways(Vector3 direction, float time)
    {
        if(isMoving) return;

        isMoving = true;
        
        AudioManager.Instance.PlayDiceSlideSFX(0.2f);

        Tween.Position(transform, transform.position + direction, time).OnComplete(target: this, target => {
            target.isMoving = false;
            OnDiceMoved?.Invoke();
        });
    }

    public void ChangeMoveAmount(int amount)
    {
        moveAmount = amount;
        moveAmountLeft = moveAmount;
        OnMoveAmountChanged?.Invoke(moveAmountLeft);
    }
}
