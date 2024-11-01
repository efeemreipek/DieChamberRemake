using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private GameControls gameControls;
    private InputAction moveForwardAction;
    private InputAction moveBackAction;
    private InputAction moveRightAction;
    private InputAction moveLeftAction;
    private InputAction helpAction;
    private InputAction restartAction;

    protected override void Awake()
    {
        base.Awake();

        gameControls = new GameControls();
    }

    private void OnEnable()
    {
        moveForwardAction = gameControls.Player.MoveForward;
        moveBackAction = gameControls.Player.MoveBack;
        moveRightAction = gameControls.Player.MoveRight;
        moveLeftAction = gameControls.Player.MoveLeft;
        helpAction = gameControls.Player.Help;
        restartAction = gameControls.Player.Restart;

        gameControls.Enable();
    }
    private void OnDisable()
    {
        gameControls.Disable();
    }

    public bool MoveForward => moveForwardAction.WasPressedThisFrame();
    public bool MoveBack => moveBackAction.WasPressedThisFrame();
    public bool MoveRight => moveRightAction.WasPressedThisFrame();
    public bool MoveLeft => moveLeftAction.WasPressedThisFrame();
    public bool Help => helpAction.IsPressed();
    public bool Restart => restartAction.IsPressed();
}
