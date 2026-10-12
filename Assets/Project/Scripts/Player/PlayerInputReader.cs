using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    /// <summary>True while the run button is held.</summary>
    public bool RunHeld { get; private set; }
    /// <summary>True while the jump button is held (for variable jump height).</summary>
    public bool JumpHeld { get; private set; }
    /// <summary>Raised when jump is pressed.</summary>
    public event Action JumpPressed;
    
    [SerializeField] private InputActionReference MoveAction;
    [SerializeField] private InputActionReference LookAction;
    [SerializeField] private InputActionReference RunAction;
    [SerializeField] private InputActionReference JumpAction;

    private void OnEnable()
    {
        MoveAction.action.performed += OnMove;
        MoveAction.action.canceled += OnStopMove;
        if (LookAction != null)
        {
            LookAction.action.performed += OnLook;
            LookAction.action.canceled += OnStopLook;
        }
        if (RunAction != null)
        {
            RunAction.action.performed += OnRun;
            RunAction.action.canceled += OnStopRun;
        }
        if (JumpAction != null)
        {
            JumpAction.action.performed += OnJump;
            JumpAction.action.canceled += OnStopJump;
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
        if (RunAction != null)
        {
            RunAction.action.performed -= OnRun;
            RunAction.action.canceled -= OnStopRun;
        }
        if (JumpAction != null)
        {
            JumpAction.action.performed -= OnJump;
            JumpAction.action.canceled -= OnStopJump;
        }
        LookInput = Vector2.zero;
        RunHeld = false;
        JumpHeld = false;
    }

    private void OnRun(InputAction.CallbackContext ctx) { RunHeld = true; }
    private void OnStopRun(InputAction.CallbackContext ctx) { RunHeld = false; }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        JumpHeld = true;
        JumpPressed?.Invoke();
    }

    private void OnStopJump(InputAction.CallbackContext ctx) { JumpHeld = false; }

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
