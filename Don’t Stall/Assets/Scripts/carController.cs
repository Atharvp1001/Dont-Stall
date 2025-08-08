using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class carController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float steeringSpeed = 150f;
    [SerializeField] private float driftFactor = 0.95f;
    [SerializeField] private float handbrakeDriftFactor = 0.8f;
    [SerializeField] private float drag = 0.98f;

    [Header("Drift Settings")]
    [SerializeField] private float traction = 1.0f;
    [SerializeField] private float handbrakeTraction = 0.3f;

    [Header("Extra Settings")]
    private Rigidbody2D rb; 
    private float steeringInput;
    private float accelerationInput;
    private bool handbrake;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get player input
        accelerationInput = Input.GetAxis("Vertical");   // W/S or Up/Down
        steeringInput = Input.GetAxis("Horizontal");     // A/D or Left/Right
        handbrake = Input.GetKey(KeyCode.Space);
    }

    void FixedUpdate()
    {
        // Apply engine force
        Vector2 forward = transform.up;
        float currentSpeed = Vector2.Dot(rb.linearVelocity, forward);

        if (accelerationInput != 0)
        {
            float speedFactor = Mathf.Clamp01(1 - (currentSpeed / maxSpeed));
            rb.AddForce(forward * accelerationInput * acceleration * speedFactor, ForceMode2D.Force);
        }
        else
        {
            // Simulate drag
            rb.linearVelocity *= drag;
        }

        // Drift
        float currentDriftFactor = handbrake ? handbrakeDriftFactor : driftFactor;
        rb.linearVelocity = ForwardVelocity() + SidewaysVelocity() * currentDriftFactor;

        // Steering
        float effectiveSteer = steeringInput * steeringSpeed * Time.fixedDeltaTime * Mathf.Sign(currentSpeed);
        rb.MoveRotation(rb.rotation - effectiveSteer);

        // Handbrake 
        if (handbrake)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.01f);
        }
    }

    // Returns only the forward (up) velocity component
    private Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
    }

    // Returns only the sideways (right) velocity component
    private Vector2 SidewaysVelocity()
    {
        return transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
    }
}
