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

    [Header("Action Maps")]
    [SerializeField] private string playerMapName = "Player";
    [SerializeField] private string uiMapName = "UI";

    /// <summary>True while the UI action map is the active one (e.g. paused).</summary>
    public bool IsUIMode { get; private set; }

    /// <summary>
    /// Switches the input between the gameplay (Player) map and the UI map.
    /// While in UI mode, gameplay input is disabled and its values are reset.
    /// </summary>
    public void SetUIMode(bool uiMode)
    {
        IsUIMode = uiMode;
        InputActionAsset asset = MoveAction.action.actionMap.asset;
        InputActionMap player = asset.FindActionMap(playerMapName, false);
        InputActionMap ui = asset.FindActionMap(uiMapName, false);

        if (uiMode)
        {
            player?.Disable(); // fires canceled callbacks -> Move/Look/Run/Jump reset
            ui?.Enable();
            MovementInput = Vector2.zero;
            LookInput = Vector2.zero;
            RunHeld = false;
            JumpHeld = false;
        }
        else
        {
            // UI map is left enabled on purpose: the EventSystem's UI input module may rely on it.
            player?.Enable();
        }
    }

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
