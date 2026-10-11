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
    }

    private void OnDestroy()
    {
        subscription?.Dispose();
    }
}
