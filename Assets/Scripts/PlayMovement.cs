using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float runSpeed = 1f;
    public float strafeSpeed = 1f;
    public float jumpForce = 350f;

    public float brakeForceMultiplier = 6f;

    public Image targetImage;
    public TextMeshProUGUI textUI;

    private Vector2 moveInput;
    private bool doJump;

    private float originalAlphaImage;
    private float originalAlphaText;

    private bool canUseEnter = true;
    private bool effectActive = false;
    private bool inCooldown = false;

    void Start()
    {
        if (targetImage != null)
            originalAlphaImage = targetImage.color.a;

        if (textUI != null)
            originalAlphaText = textUI.color.a;
    }

    void Update()
    {
        HandleInput();
        HandleEnter();
        HandleJumpInput();
        HandleFallCheck();
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Brake();
    }


    void HandleInput()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed) horizontal = 1f;
            else if (Keyboard.current.aKey.isPressed) horizontal = -1f;

            if (Keyboard.current.wKey.isPressed) vertical = 1f;
            else if (Keyboard.current.sKey.isPressed) vertical = -1f;
        }

        moveInput = new Vector2(horizontal, vertical);
    }

    void HandleJumpInput()
    {
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame &&
            IsGrounded())
        {
            doJump = true;
        }
    }

    void HandleEnter()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.enterKey.wasPressedThisFrame && canUseEnter && !inCooldown)
        {
            StartCoroutine(EnterRoutine());
        }
    }

    void HandleFallCheck()
    {
        if (transform.position.y < 0f)
        {
            FindFirstObjectByType<GameManager>().RestartLevel();
        }
    }


    void Move()
    {
        rb.AddForce(moveInput.x * strafeSpeed, 0f, 0f, ForceMode.VelocityChange);
        rb.AddForce(0f, 0f, moveInput.y * runSpeed, ForceMode.VelocityChange);
    }

    void Jump()
    {
        if (doJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doJump = false;
        }
    }

    void Brake()
    {
        if (!effectActive) return;

        if (Keyboard.current != null &&
            Keyboard.current.enterKey.isPressed &&
            IsGrounded())
        {
            Vector3 velocity = rb.linearVelocity;
            velocity.y = 0f;

            Vector3 brakeForce = -velocity * brakeForceMultiplier;
            rb.AddForce(brakeForce, ForceMode.Acceleration);
        }
    }


    IEnumerator EnterRoutine()
    {
        canUseEnter = false;
        effectActive = true;

        SetTransparency(0.5f);

        yield return new WaitForSeconds(1f);

        effectActive = false;

        inCooldown = true;

        yield return new WaitForSeconds(5f);

        inCooldown = false;
        canUseEnter = true;

        SetTransparency(originalAlphaImage);
    }

    void SetTransparency(float alpha)
    {
        if (targetImage != null)
        {
            Color c = targetImage.color;
            c.a = alpha;
            targetImage.color = c;
        }

        if (textUI != null)
        {
            Color c = textUI.color;
            c.a = alpha;
            textUI.color = c;
        }
    }


    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}