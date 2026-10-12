using UnityEngine;
using VContainer;

/// <summary>
/// Stage 1 player movement. Reads processed input from <see cref="PlayerInputReader"/>
/// and moves the Rigidbody on the XZ plane with acceleration, a speed cap, run, jump
/// (coyote time, jump buffer, variable height) and facing rotation.
/// Tunable values live in a <see cref="PlayerMovementStats"/> ScriptableObject.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rg;

    [Header("Stats")]
    [SerializeField] private PlayerMovementStats stats;

    [Header("Optional")]
    [Tooltip("If assigned, movement is relative to this transform's yaw (e.g. the Main Camera).")]
    [SerializeField] private Transform cameraTransform;
    [Tooltip("Collider used to find the feet for the ground check. Auto-found if empty.")]
    [SerializeField] private Collider bodyCollider;

    private PlayerInputReader inputReader;
    private bool subscribed;

    private bool isGrounded;
    private float lastGroundedTime = float.NegativeInfinity;
    private float lastJumpPressedTime = float.NegativeInfinity;

    /// <summary>True when standing on ground.</summary>
    public bool IsGrounded => isGrounded;
    /// <summary>True while the run button is held and the player is moving.</summary>
    public bool IsRunning { get; private set; }

    [Inject]
    public void Construct(PlayerInputReader playerInputReader)
    {
        inputReader = playerInputReader;
    }

    private void Reset()
    {
        rg = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<Collider>();
    }

    private void Awake()
    {
        if (rg != null)
        {
            rg.freezeRotation = true; // we rotate manually; prevents tipping over
        }

        if (bodyCollider == null)
        {
            bodyCollider = GetComponent<Collider>();
        }
    }

    private void Start()
    {
        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (subscribed || inputReader == null)
        {
            return;
        }

        inputReader.JumpPressed += OnJumpPressed;
        subscribed = true;
    }

    private void Unsubscribe()
    {
        if (!subscribed || inputReader == null)
        {
            return;
        }

        inputReader.JumpPressed -= OnJumpPressed;
        subscribed = false;
    }

    private void OnJumpPressed()
    {
        lastJumpPressedTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (stats == null || inputReader == null)
        {
            return;
        }

        CheckGround();
        HandleHorizontalMovement();
        HandleJump();
        ApplyExtraGravity();
        HandleRotation();
    }

    private void CheckGround()
    {
        Vector3 origin;
        float castDistance;
        if (bodyCollider != null)
        {
            Bounds b = bodyCollider.bounds;
            origin = b.center;
            castDistance = b.extents.y - stats.groundCheckRadius + stats.groundCheckDistance;
        }
        else
        {
            origin = rg.position + Vector3.up * stats.groundCheckRadius;
            castDistance = stats.groundCheckDistance;
        }

        isGrounded = Physics.SphereCast(origin, stats.groundCheckRadius, Vector3.down, out _,
            Mathf.Max(0f, castDistance), stats.groundMask, QueryTriggerInteraction.Ignore);

        // Ignore "grounded" while moving upward (e.g. right after jumping).
        if (isGrounded && rg.linearVelocity.y <= 0.01f)
        {
            lastGroundedTime = Time.time;
        }
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
        IsRunning = inputReader.RunHeld && direction != Vector3.zero;

        Vector3 velocity = rg.linearVelocity;
        Vector3 horizontal = new Vector3(velocity.x, 0f, velocity.z);

        float speed = IsRunning ? stats.runSpeed : stats.moveSpeed;
        Vector3 targetVelocity = direction * speed;
        float rate = direction == Vector3.zero ? stats.deceleration : stats.acceleration;
        if (!isGrounded)
        {
            rate *= stats.airControl;
        }

        Vector3 newHorizontal = Vector3.MoveTowards(horizontal, targetVelocity, rate * Time.fixedDeltaTime);

        // Preserve vertical velocity (gravity / jumps)
        rg.linearVelocity = new Vector3(newHorizontal.x, velocity.y, newHorizontal.z);
    }

    private void HandleJump()
    {
        bool canCoyote = Time.time - lastGroundedTime <= stats.coyoteTime;
        bool buffered = Time.time - lastJumpPressedTime <= stats.jumpBufferTime;

        if (!buffered || !canCoyote)
        {
            return;
        }

        // v = sqrt(2 * g * h), using the effective upward gravity
        float gravity = Mathf.Abs(Physics.gravity.y);
        float jumpVelocity = Mathf.Sqrt(2f * gravity * stats.jumpHeight);

        Vector3 velocity = rg.linearVelocity;
        rg.linearVelocity = new Vector3(velocity.x, jumpVelocity, velocity.z);

        // Consume buffer and coyote so we can't double jump
        lastJumpPressedTime = float.NegativeInfinity;
        lastGroundedTime = float.NegativeInfinity;
    }

    private void ApplyExtraGravity()
    {
        if (isGrounded)
        {
            return;
        }

        float vy = rg.linearVelocity.y;
        float multiplier = 1f;
        if (vy < 0f)
        {
            multiplier = stats.fallGravityMultiplier;
        }
        else if (vy > 0f && !inputReader.JumpHeld)
        {
            multiplier = stats.lowJumpGravityMultiplier;
        }

        if (multiplier > 1f)
        {
            // Extra acceleration on top of Unity's normal gravity
            rg.AddForce(Physics.gravity * (multiplier - 1f), ForceMode.Acceleration);
        }
    }

    private void HandleRotation()
    {
        Vector3 direction = GetMoveDirection();
        if (direction == Vector3.zero)
        {
            return;
        }

        Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
        rg.MoveRotation(Quaternion.Slerp(rg.rotation, target, stats.rotationSpeed * Time.fixedDeltaTime));
    }
}
