using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

/// <summary>
/// Stage 1 win/lose rules.
/// Win : timer hits 0 AND score > ScoreManager.WinScore.
/// Lose: timer hits 0 with score <= WinScore, OR the player falls below fallThresholdY.
/// Shows a result panel, freezes the game and offers Retry / Main Menu.
/// </summary>
public class GameResultController : MonoBehaviour
{
    [Header("Fall (pitfall) detection")]
    [Tooltip("Player Y below this value = fell off the map.")]
    [SerializeField] private float fallThresholdY = -10f;

    [Header("Win panel")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winScoreText;

    [Header("Game Over panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverReasonText;

    [Header("Scenes")]
    [SerializeField] private int mainMenuSceneIndex = 0;

    private ScoreManager scoreManager;
    private TimeCounter timeCounter;
    private PlayerMovement player;
    private bool finished;

    /// <summary>True once the stage has been won or lost.</summary>
    public bool IsFinished => finished;

    [Inject]
    public void Construct(ScoreManager scoreManager, TimeCounter timeCounter, PlayerMovement player)
    {
        this.scoreManager = scoreManager;
        this.timeCounter = timeCounter;
        this.player = player;
        timeCounter.TimeExpired += OnTimeExpired;
    }

    private void Start()
    {
        Time.timeScale = 1f; // in case a previous run ended while paused
        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (finished || player == null) return;
        if (player.transform.position.y < fallThresholdY)
        {
            Lose("You fell off the stage!");
        }
    }

    private void OnDestroy()
    {
        if (timeCounter != null) timeCounter.TimeExpired -= OnTimeExpired;
        Time.timeScale = 1f;
    }

    private void OnTimeExpired()
    {
        if (finished) return;

        if (scoreManager.Score > ScoreManager.WinScore) Win();
        else Lose("Time's up! Score too low.");
    }

    private void Win()
    {
        if (!Finish()) return;
        if (winScoreText != null) winScoreText.SetText($"Final Score: {scoreManager.Score}");
        if (winPanel != null) winPanel.SetActive(true);
        SoundManager.Instance?.PlaySFX("Win"); 
    }

    private void Lose(string reason)
    {
        if (!Finish()) return;
        if (gameOverScoreText != null) gameOverScoreText.SetText($"Final Score: {scoreManager.Score}");
        if (gameOverReasonText != null) gameOverReasonText.SetText(reason);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        SoundManager.Instance?.PlaySFX("Lose");
    }

    // Returns false if the game already ended (only the first result counts).
    private bool Finish()
    {
        if (finished) return false;
        finished = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        return true;
    }

    // ---- Hook these to the panel buttons (Button > OnClick) ----

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
