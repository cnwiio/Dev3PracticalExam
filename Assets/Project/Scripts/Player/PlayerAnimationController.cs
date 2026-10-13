using UnityEngine;

/// <summary>
/// Drives the player's Animator (Idle / Walk / Run / Jump / Fall) in Stage 2.
/// Purely a "view" layer: it only READS state from <see cref="PlayerMovement"/> and the Rigidbody,
/// so the Stage 1 movement scripts are reused unchanged. Add this next to PlayerMovement
/// and point it at the Animator on the character model.
/// Parameters (all optional - missing ones are skipped silently):
///   Speed (float), IsGrounded (bool), IsRunning (bool), VerticalSpeed (float), Jump (trigger)
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Rigidbody rg;

    [Header("Tuning")]
    [Tooltip("Smoothing time for the Speed parameter (blends Idle/Walk/Run).")]
    [SerializeField] private float speedDampTime = 0.1f;
    [Tooltip("Upward velocity (m/s) required to count as a jump takeoff.")]
    [SerializeField] private float jumpTakeoffVelocity = 1f;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int VerticalSpeedHash = Animator.StringToHash("VerticalSpeed");
    private static readonly int JumpHash = Animator.StringToHash("Jump");

    private bool hasSpeed, hasGrounded, hasRunning, hasVertical, hasJump;
    private bool wasGrounded = true;

    private void Reset()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<PlayerMovement>();
        rg = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (rg == null) rg = GetComponent<Rigidbody>();

        CacheParameters();
    }

    private void CacheParameters()
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            return;
        }

        foreach (AnimatorControllerParameter p in animator.parameters)
        {
            if (p.nameHash == SpeedHash) hasSpeed = true;
            else if (p.nameHash == IsGroundedHash) hasGrounded = true;
            else if (p.nameHash == IsRunningHash) hasRunning = true;
            else if (p.nameHash == VerticalSpeedHash) hasVertical = true;
            else if (p.nameHash == JumpHash) hasJump = true;
        }
    }

    private void Update()
    {
        if (animator == null || movement == null || rg == null)
        {
            return;
        }

        Vector3 v = rg.linearVelocity;
        float horizontalSpeed = new Vector3(v.x, 0f, v.z).magnitude;
        bool grounded = movement.IsGrounded;

        if (hasSpeed)
        {
            animator.SetFloat(SpeedHash, horizontalSpeed, speedDampTime, Time.deltaTime);
        }
        if (hasGrounded)
        {
            animator.SetBool(IsGroundedHash, grounded);
        }
        if (hasRunning)
        {
            animator.SetBool(IsRunningHash, movement.IsRunning);
        }
        if (hasVertical)
        {
            animator.SetFloat(VerticalSpeedHash, v.y);
        }

        // Jump takeoff: left the ground while moving upward
        if (hasJump && wasGrounded && !grounded && v.y > jumpTakeoffVelocity)
        {
            animator.SetTrigger(JumpHash);
        }

        wasGrounded = grounded;
    }
}
