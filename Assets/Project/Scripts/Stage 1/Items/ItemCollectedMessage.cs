/// <summary>
/// MessagePipe message published when the player touches an item.
/// Subscribers: TimeCounter (time), ScoreManager (score), ItemSfxPlayer (sound).
/// </summary>
public readonly struct ItemCollectedMessage
{
    public readonly ItemType Type;
    public readonly int ScoreDelta;     // can be negative
    public readonly float TimeDelta;    // seconds, can be negative

    public ItemCollectedMessage(ItemType type, int scoreDelta, float timeDelta)
    {
        Type = type;
        ScoreDelta = scoreDelta;
        TimeDelta = timeDelta;
    }

    public bool IsPositive => Type == ItemType.TimeBoost || Type == ItemType.ScoreBoost;
}
