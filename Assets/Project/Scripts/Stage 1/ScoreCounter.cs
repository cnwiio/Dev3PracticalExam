using TMPro;
using UnityEngine;
using VContainer;

/// <summary>HUD score text for Stage 1.</summary>
public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;

    private ScoreManager scoreManager;

    [Inject]
    public void Construct(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
        scoreManager.OnScoreChanged += Refresh;
        Refresh(scoreManager.Score);
    }

    private void OnDestroy()
    {
        if (scoreManager != null) scoreManager.OnScoreChanged -= Refresh;
    }

    private void Refresh(int score)
    {
        textUI.SetText($"Score: {score}");
    }
}
