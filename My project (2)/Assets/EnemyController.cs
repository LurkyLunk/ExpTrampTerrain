using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Transform target; // Reference to the player's transform

    public float movementSpeed = 5f;
    public float rotationSpeed = 5f;
    public float dashForce = 10f;
    public float dashDuration = 0.5f;
    public float doubleJumpForce = 10f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isDashing = false;
    private bool canDoubleJump = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (target != null)
        {
            // Rotate towards the player
            Vector3 targetDirection = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move towards the player
            Vector3 movement = transform.forward * movementSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + movement);

            // Dash input
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (!isDashing)
                {
                    StartCoroutine(Dash());
                }
            }

            // Double jump input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isGrounded && canDoubleJump)
                {
                    rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
                    canDoubleJump = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Bounce off the player upon collision
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 direction = (transform.position - collision.transform.position).normalized;
            Vector3 bounceForce = direction * 5f;
            rb.AddForce(bounceForce, ForceMode.Impulse);
        }

        // Reset double jump and grounded status
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            canDoubleJump = true;
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
}


