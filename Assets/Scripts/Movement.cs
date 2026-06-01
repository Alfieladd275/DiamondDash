using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public float moveSpeed = 6f;
    public float jumpForce = 14f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }

        _animator.SetBool("isRunning", moveInput != 0);

        // Jump
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // ⭐ Explicit true/false for jump animation ⭐
        if (isGrounded)
        {
            _animator.SetBool("isJumping", false);
        }
        else
        {
            _animator.SetBool("isJumping", true);
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
