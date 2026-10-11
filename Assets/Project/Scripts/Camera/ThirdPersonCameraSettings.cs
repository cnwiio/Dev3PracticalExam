using UnityEngine;

/// <summary>
/// Adjustable stats for the third-person camera (ScriptableObject).
/// Create via Assets > Create > Project > Third Person Camera Settings.
/// </summary>
[CreateAssetMenu(fileName = "ThirdPersonCameraSettings", menuName = "Project/Third Person Camera Settings")]
public class ThirdPersonCameraSettings : ScriptableObject
{
    [Header("Look")]
    [Tooltip("Degrees per input unit for mouse delta.")]
    public float sensitivity = 0.12f;
    public bool invertY = false;
    public float minPitch = -30f;
    public float maxPitch = 70f;

    [Header("Framing")]
    [Tooltip("Height of the pivot above the player's origin.")]
    public Vector3 pivotOffset = new Vector3(0f, 1.6f, 0f);
    public float cameraDistance = 5f;
    public Vector3 shoulderOffset = new Vector3(0.5f, -0.2f, 0f);
    [Range(0f, 1f)] public float cameraSide = 0.6f;
    public float verticalArmLength = 0f;

    [Header("Smoothing")]
    public Vector3 damping = new Vector3(0.1f, 0.25f, 0.3f);
    [Tooltip("Smoothing of look rotation. 0 = instant.")]
    public float rotationSmoothTime = 0.03f;

    [Header("Collision")]
    public LayerMask collisionMask = ~0;
    public float cameraRadius = 0.25f;
    public float dampingIntoCollision = 0f;
    public float dampingFromCollision = 0.5f;
}
