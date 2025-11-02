using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform PlayerGroundFeet;
    [SerializeField] private float groundCheckRadius = 1f;

    private float moveInput;
    private bool isGrounded;
    private bool jumpPressed;
    private bool isRunning;
    private bool attackPressed;
    private bool isAttacking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            attackPressed = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(PlayerGroundFeet.position, groundCheckRadius, LayerMask.GetMask("Ground")) != null;

        Move(isRunning ? speed * 1.5f : speed);

        UpdateAnimation(moveInput != 0, isRunning);

        Jump();
        Attack();
    }

    private void Move(float speed)
    {
        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void Jump()
    {
        if (jumpPressed && isGrounded && rb.linearVelocity.y <= 0.1f && !attackPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }
    }

    private void UpdateAnimation(bool isWalking, bool isRunning)
    {
        if (isWalking && !attackPressed)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            animator.SetBool("isWalking", !isRunning);
            animator.SetBool("isRunning", isRunning);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

    }

    private void Attack()
    {
        if (attackPressed && isGrounded)
        {
            animator.SetBool("isAttacking", true);
            isAttacking = true;
            attackPressed = false;
        }
    }

    private void OnAttackAnimationEnd()
    {
        animator.SetBool("isAttacking", false);
        isAttacking = false;
    }
}
