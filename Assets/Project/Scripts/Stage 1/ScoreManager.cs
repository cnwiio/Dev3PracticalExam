using System;
using MessagePipe;

/// <summary>Holds the Stage 1 score; updated by ItemCollectedMessage.</summary>
public class ScoreManager : IDisposable
{
    public const int WinScore = 10000;

    public int Score { get; private set; }
    public event Action<int> OnScoreChanged;

    private readonly IDisposable subscription;

    public ScoreManager(ISubscriber<ItemCollectedMessage> subscriber)
    {
        subscription = subscriber.Subscribe(OnItemCollected);
    }

    private void OnItemCollected(ItemCollectedMessage msg)
    {
        if (msg.ScoreDelta == 0) return;
        Score += msg.ScoreDelta;
        OnScoreChanged?.Invoke(Score);
    }

    public void Dispose()
    {
        subscription.Dispose();
    }
}
