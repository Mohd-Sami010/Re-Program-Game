using UnityEngine;

public class GroundCheck :MonoBehaviour {

    public static GroundCheck Instance { get; private set; }

    [Tooltip("Maximum distance from the ground.")]
    [SerializeField] private float groundCheckRadius = .15f;
    [SerializeField] private LayerMask groundLayerMask;

    [Tooltip("Whether this transform is grounded now.")]
    private bool isGrounded = false;
    public event System.Action OnGrounded;

    private void Awake()
    {
        Instance = this;
    }
    void LateUpdate()
    {
        // Check if we are grounded now.
        bool isGroundedNow = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayerMask);

        // Call event if we were in the air and we are now touching the ground.
        if (isGroundedNow && !isGrounded)
        {
            OnGrounded?.Invoke();
        }

        // Update isGrounded.
        isGrounded = isGroundedNow;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}
