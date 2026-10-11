using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Scriptable Objects/PlayerMovementData")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float acceleration = 40f;
    public float deceleration = 50f;
    [Header("Rotation")]
    public float rotationSpeed = 12f;
}
