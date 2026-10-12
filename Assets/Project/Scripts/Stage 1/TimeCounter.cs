using System;
using MessagePipe;
using TMPro;
using UnityEngine;
using VContainer;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private float startTime = 30f;

    private float counterF;
    private int counter;
    private IDisposable subscription;
    private bool expired;

    /// <summary>Raised once when the countdown reaches 0 (including via a time penalty).</summary>
    public event Action TimeExpired;

    [Inject]
    public void Construct(ISubscriber<ItemCollectedMessage> subscriber)
    {
        // Time boost / penalty items arrive through MessagePipe
        subscription = subscriber.Subscribe(OnItemCollected);
    }

    private void Start()
    {
        counterF = startTime;
    }

    private void Update()
    {
        if (counterF <= 0f) return;

        counterF -= Time.deltaTime;
        Refresh();
    }

    private void OnItemCollected(ItemCollectedMessage msg)
    {
        if (msg.TimeDelta == 0f) return;
        counterF = Mathf.Max(0f, counterF + msg.TimeDelta);
        Refresh();
    }

    private void Refresh()
    {
        counterF = Mathf.Max(0f, counterF);
        counter = (int)MathF.Ceiling(counterF);
        textUI.SetText($"-- {counter.ToString()} --");

        if (counterF <= 0f && !expired)
        {
            expired = true;
            TimeExpired?.Invoke();
        }
    }

    private void OnDestroy()
    {
        subscription?.Dispose();
    }
}
