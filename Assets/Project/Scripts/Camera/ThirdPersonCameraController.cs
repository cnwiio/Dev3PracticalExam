using Unity.Cinemachine;
using UnityEngine;
using VContainer;

/// <summary>
/// Drives a Cinemachine 3 third-person camera.
/// Put this on an empty "CameraTarget" object (child of the Player, or anywhere) and set it as the
/// CinemachineCamera's Tracking Target. The controller follows the player, rotates from Look input,
/// and pushes values from <see cref="ThirdPersonCameraSettings"/> into CinemachineThirdPersonFollow.
/// </summary>
public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField] private ThirdPersonCameraSettings settings;
    [SerializeField] private Transform player;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private bool lockCursor = true;

    private PlayerInputReader inputReader;
    private CinemachineThirdPersonFollow follow;

    private float yaw;
    private float pitch;
    private float smoothYaw;
    private float smoothPitch;
    private float yawVelocity;
    private float pitchVelocity;

    /// <summary>Camera yaw, usable for camera-relative movement.</summary>
    public float Yaw => yaw;

    [Inject]
    public void Construct(PlayerInputReader playerInputReader)
    {
        inputReader = playerInputReader;
    }

    private void Awake()
    {
        if (cinemachineCamera != null)
        {
            follow = cinemachineCamera.GetComponent<CinemachineThirdPersonFollow>();
        }

        Vector3 angles = transform.eulerAngles;
        yaw = smoothYaw = angles.y;
        pitch = smoothPitch = NormalizePitch(angles.x);
    }

    private void OnEnable()
    {
        SetCursorLocked(lockCursor);
    }

    private void OnDisable()
    {
        SetCursorLocked(false);
    }

    private void LateUpdate()
    {
        if (settings == null || player == null)
        {
            return;
        }

        ApplySettings();
        HandleLook();

        transform.position = player.position + settings.pivotOffset;
        transform.rotation = Quaternion.Euler(smoothPitch, smoothYaw, 0f);
    }

    private void HandleLook()
    {
        Vector2 look = inputReader != null ? inputReader.LookInput : Vector2.zero;

        yaw += look.x * settings.sensitivity;
        pitch += (settings.invertY ? look.y : -look.y) * settings.sensitivity;
        pitch = Mathf.Clamp(pitch, settings.minPitch, settings.maxPitch);

        if (settings.rotationSmoothTime <= 0f)
        {
            smoothYaw = yaw;
            smoothPitch = pitch;
            return;
        }

        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawVelocity, settings.rotationSmoothTime);
        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchVelocity, settings.rotationSmoothTime);
    }

    // Applied every frame so values can be tuned live in the ScriptableObject while playing.
    private void ApplySettings()
    {
        if (follow == null)
        {
            return;
        }

        follow.CameraDistance = settings.cameraDistance;
        follow.ShoulderOffset = settings.shoulderOffset;
        follow.CameraSide = settings.cameraSide;
        follow.VerticalArmLength = settings.verticalArmLength;
        follow.Damping = settings.damping;

        // Collision settings live in the nested ObstacleSettings struct (copy, modify, assign back).
        CinemachineThirdPersonFollow.ObstacleSettings obstacles = follow.AvoidObstacles;
        obstacles.CollisionFilter = settings.collisionMask;
        obstacles.CameraRadius = settings.cameraRadius;
        obstacles.DampingIntoCollision = settings.dampingIntoCollision;
        obstacles.DampingFromCollision = settings.dampingFromCollision;
        follow.AvoidObstacles = obstacles;
    }

    private static float NormalizePitch(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }

    private static void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
