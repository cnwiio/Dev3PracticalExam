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

    [Header("Run")]
    [Tooltip("Speed while the run button is held.")]
    public float runSpeed = 10f;

    [Header("Rotation")]
    public float rotationSpeed = 12f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    [Tooltip("Gravity multiplier while falling (snappier landings).")]
    public float fallGravityMultiplier = 2f;
    [Tooltip("Gravity multiplier when jump is released early while rising (variable jump height).")]
    public float lowJumpGravityMultiplier = 2.5f;
    [Tooltip("Grace time to still jump after leaving a ledge.")]
    public float coyoteTime = 0.1f;
    [Tooltip("Time a jump press is remembered before landing.")]
    public float jumpBufferTime = 0.1f;
    [Range(0f, 1f)] public float airControl = 0.5f;

    [Header("Ground Check")]
    public LayerMask groundMask = ~0;
    public float groundCheckDistance = 0.15f;
    public float groundCheckRadius = 0.25f;
}

