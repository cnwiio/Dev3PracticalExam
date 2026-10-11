using UnityEngine;
using VContainer;

/// <summary>
/// Stage 1 player movement. Reads processed input from <see cref="PlayerInputReader"/>
/// and moves the Rigidbody on the XZ plane with acceleration, a speed cap, and facing rotation.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Required")]
    [SerializeField] private Rigidbody rg;

    [Header("Movement Data")]
    [SerializeField] private PlayerMovementData data;
    

    [Header("Optional")]
    [Tooltip("If assigned, movement is relative to this transform's yaw (e.g. the camera).")]
    [SerializeField] private Transform cameraTransform;

    private PlayerInputReader inputReader;

    [Inject]
    public void Construct(PlayerInputReader playerInputReader)
    {
        inputReader = playerInputReader;
    }

    private void Reset()
    {
        rg = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if (rg != null)
        {
            rg.freezeRotation = true; // we rotate manually; prevents tipping over
        }
    }

    private void FixedUpdate()
    {
        HandleHorizontalMovement();
        HandleRotation();
    }

    private Vector3 GetMoveDirection()
    {
        Vector2 input = inputReader.MovementInput;
        Vector3 dir = new Vector3(input.x, 0f, input.y);
        if (dir.sqrMagnitude < 0.001f)
        {
            return Vector3.zero;
        }

        if (cameraTransform != null)
        {
            Quaternion yaw = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
            dir = yaw * dir;
        }

        return Vector3.ClampMagnitude(dir, 1f);
    }

    private void HandleHorizontalMovement()
    {
        Vector3 direction = GetMoveDirection();
        Vector3 velocity = rg.linearVelocity;
        Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);

        Vector3 targetVelocity = direction * data.moveSpeed;
        float rate = direction == Vector3.zero ? data.deceleration : data.acceleration;
        Vector3 newHorizontal = Vector3.MoveTowards(horizontal, targetVelocity, rate * Time.fixedDeltaTime);

        // Preserve vertical velocity (gravity / jumps)
        rg.linearVelocity = new Vector3(newHorizontal.x, velocity.y, newHorizontal.z);
    }

    private void HandleRotation()
    {
        Vector3 direction = GetMoveDirection();
        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
        rg.MoveRotation(Quaternion.Slerp(rg.rotation, target, data.rotationSpeed * Time.fixedDeltaTime));
    }
}
