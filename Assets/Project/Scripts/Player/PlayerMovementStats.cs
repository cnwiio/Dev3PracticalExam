using UnityEngine;

/// <summary>
/// Adjustable player movement stats (ScriptableObject).
/// Create via Assets > Create > Project > Player Movement Stats.
/// </summary>
[CreateAssetMenu(fileName = "PlayerMovementStats", menuName = "Project/Player Movement Stats")]
public class PlayerMovementStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float acceleration = 40f;
    public float deceleration = 50f;

    [Header("Rotation")]
    public float rotationSpeed = 12f;
}
