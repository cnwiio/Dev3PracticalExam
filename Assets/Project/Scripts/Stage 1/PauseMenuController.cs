using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using VContainer;

/// <summary>
/// Stage 1 pause menu: Retry / Setting / Back to Main Menu.
/// Pausing freezes the game (Time.timeScale = 0), switches input to the UI action map and
/// unlocks the cursor. Resuming restores everything. Esc / gamepad Start toggles pause.
/// </summary>
public class PauseMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [Tooltip("Existing Setting Panel prefab instance (opened from the Setting button).")]
    [SerializeField] private GameObject settingPanel;

    [Header("Scenes")]
    [SerializeField] private int mainMenuSceneIndex = 0;

    private PlayerInputReader inputReader;
    private GameResultController result;
    private InputAction pauseAction;
    private bool paused;

    public bool IsPaused => paused;

    [Inject]
    public void Construct(PlayerInputReader inputReader, GameResultController result)
    {
        this.inputReader = inputReader;
        this.result = result;
    }

    private void Awake()
    {
        // Own action so pause works regardless of which action map is enabled.
        pauseAction = new InputAction("Pause", InputActionType.Button);
        pauseAction.AddBinding("<Keyboard>/escape");
        pauseAction.AddBinding("<Gamepad>/start");
    }

    private void OnEnable()
    {
        pauseAction.performed += OnPausePressed;
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPausePressed;
        pauseAction.Disable();
    }

    private void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        pauseAction?.Dispose();
        if (paused) Time.timeScale = 1f;
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (result != null && result.IsFinished) return;

        if (!paused) Pause();
        else if (settingPanel != null && settingPanel.activeSelf) CloseSettings();
        else Resume();
    }

    public void Pause()
    {
        if (paused) return;
        paused = true;

        Time.timeScale = 0f;
        inputReader.SetUIMode(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pausePanel.SetActive(true);
    }

    // ---- Hook these to the buttons (Button > OnClick) ----

    public void Resume()
    {
        if (!paused) return;
        paused = false;

        if (settingPanel != null) settingPanel.SetActive(false);
        pausePanel.SetActive(false);
        inputReader.SetUIMode(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenSettings()
    {
        if (settingPanel == null) return;
        pausePanel.SetActive(false);
        settingPanel.SetActive(true);
    }

    /// <summary>Hook to the Setting panel's Back button (or Esc while settings are open).</summary>
    public void CloseSettings()
    {
        if (settingPanel != null) settingPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
