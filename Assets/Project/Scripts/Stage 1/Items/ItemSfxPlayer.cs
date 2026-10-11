using System;
using MessagePipe;
using VContainer.Unity;

/// <summary>
/// Plays SFX on item pickup. Add Sound entries named "ItemCollect" and "ItemHit"
/// to SoundManager.sfxSounds.
/// </summary>
public class ItemSfxPlayer : IStartable, IDisposable
{
    private readonly ISubscriber<ItemCollectedMessage> subscriber;
    private IDisposable subscription;

    public ItemSfxPlayer(ISubscriber<ItemCollectedMessage> subscriber)
    {
        this.subscriber = subscriber;
    }

    public void Start()
    {
        subscription = subscriber.Subscribe(msg =>
        {
            if (SoundManager.Instance == null) return;
            SoundManager.Instance.PlaySFX(msg.IsPositive ? "ItemCollect" : "ItemHit");
        });
    }

    public void Dispose()
    {
        subscription?.Dispose();
    }
}
