using UnityEngine;
using UnityEngine.InputSystem;

public class PlayMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float runSpeed = 100f;
    public float strafeSpeed = 60f;
    public float jumpForce = 15f;

    public float brakeForceMultiplier = 6f;

    private Vector2 moveInput;
    private bool doJump;

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.dKey.isPressed) horizontal = 1f;
        else if (Keyboard.current.aKey.isPressed) horizontal = -1f;

        if (Keyboard.current.wKey.isPressed) vertical = 1f;
        else if (Keyboard.current.sKey.isPressed) vertical = -1f;

        moveInput = new Vector2(horizontal, vertical);

        if (Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            doJump = true;
        }

        if (transform.position.y < 0f)
        {
            FindObjectOfType<GameManager>().RestartLevel();
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(moveInput.x * strafeSpeed * Time.deltaTime, 0f, 0f, ForceMode.VelocityChange);
        rb.AddForce(0f, 0f, moveInput.y * runSpeed * Time.deltaTime, ForceMode.VelocityChange);

        if (doJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doJump = false;
        }

        if (Keyboard.current.enterKey.isPressed && IsGrounded())
        {
            Vector3 velocity = rb.linearVelocity;

            velocity.y = 0f;

            Vector3 brakeForce = -velocity * brakeForceMultiplier;

            rb.AddForce(brakeForce, ForceMode.Acceleration);
        }
    }
}