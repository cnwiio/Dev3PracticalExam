using MessagePipe;
using UnityEngine;
using VContainer;

/// <summary>
/// Pickup/obstacle for Stage 1. Needs a Collider with "Is Trigger" on.
/// Publishes an ItemCollectedMessage via MessagePipe when the capsule player touches it.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private int scoreAmount = 500;
    [SerializeField] private float timeAmount = 3f;

    private IPublisher<ItemCollectedMessage> publisher;
    private bool collected;

    [Inject]
    public void Construct(IPublisher<ItemCollectedMessage> publisher)
    {
        this.publisher = publisher;
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected || publisher == null) return;
        if (other.GetComponentInParent<PlayerMovement>() == null) return;

        collected = true;
        publisher.Publish(BuildMessage());
        Destroy(gameObject);
    }

    // Amounts are absolute in the inspector; sign is decided by the item type.
    private ItemCollectedMessage BuildMessage()
    {
        int score = Mathf.Abs(scoreAmount);
        float time = Mathf.Abs(timeAmount);

        switch (itemType)
        {
            case ItemType.TimeBoost:    return new ItemCollectedMessage(itemType, 0, time);
            case ItemType.ScoreBoost:   return new ItemCollectedMessage(itemType, score, 0f);
            case ItemType.TimePenalty:  return new ItemCollectedMessage(itemType, 0, -time);
            default:                    return new ItemCollectedMessage(itemType, -score, 0f);
        }
    }
}
