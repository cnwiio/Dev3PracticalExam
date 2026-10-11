using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    
    [SerializeField] private InputActionReference MoveAction;
    [SerializeField] private InputActionReference LookAction;

    private void OnEnable()
    {
        MoveAction.action.performed += OnMove;
        MoveAction.action.canceled += OnStopMove;
        if (LookAction != null)
        {
            LookAction.action.performed += OnLook;
            LookAction.action.canceled += OnStopLook;
        }
    }

    private void OnDisable()
    {
        MoveAction.action.performed -= OnMove;
        MoveAction.action.canceled -= OnStopMove;
        if (LookAction != null)
        {
            LookAction.action.performed -= OnLook;
            LookAction.action.canceled -= OnStopLook;
        }
        LookInput = Vector2.zero;
    }

    private void OnLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
    }

    private void OnStopLook(InputAction.CallbackContext ctx)
    {
        LookInput = Vector2.zero;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        MovementInput = ctx.ReadValue<Vector2>();
    }
    private void OnStopMove(InputAction.CallbackContext ctx)
    {
        MovementInput = Vector2.zero;
    }
}
