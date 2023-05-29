using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public int maxJumps = 2;
    public float rotationSpeed = 180f; // Rotation speed in degrees per second

    private Rigidbody rb;
    private Vector3 moveDirection;
    private int jumpsRemaining;
    private bool isDashing;
    private float dashTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        // Read input for movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Jump input detection
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Dash input detection
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isDashing)
            {
                Dash();
            }
        }

        // Rotate the player when 'A' or 'D' keys are pressed
        if (Input.GetKey(KeyCode.A))
        {
            RotatePlayer(-rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotatePlayer(rotationSpeed);
        }

        // Check if dash duration has elapsed
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Move the player based on the current movement direction and speed
        if (!isDashing)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + moveDirection * dashSpeed * Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        if (jumpsRemaining > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsRemaining--;
        }
    }

    private void Dash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        rb.velocity = Vector3.zero;
        rb.AddForce(moveDirection * dashSpeed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Reset the number of jumps and dashing status when the player touches the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsRemaining = maxJumps;
            isDashing = false;
        }
    }

    private void RotatePlayer(float rotationAmount)
    {
        // Rotate the player around the Y-axis by the specified rotation amount
        transform.Rotate(Vector3.up * rotationAmount * Time.deltaTime);
    }
}
