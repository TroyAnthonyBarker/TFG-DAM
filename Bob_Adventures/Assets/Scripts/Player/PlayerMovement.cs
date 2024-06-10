using UnityEngine;
using UnityEngine.InputSystem;

public class BobMovements : MonoBehaviour
{
    // References
    private Rigidbody2D rb;
    private Animator Animator;
    private AudioSource source;
    private Door door;

    [Header("Movement")]
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float speedMultiplier = 2f;
    private float horizontal;
    private float runningSpeed;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Climbing")]
    [SerializeField] private float climbSpeed = 5f;
    private float vertical;
    private bool isClimbing;
    private bool isLadder;

    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;

    [Header("GroundCheck")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.8f, 0.08f);
    [SerializeField] private LayerMask groundLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip climbSound;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        runningSpeed = speed;
    }

    void Update()
    {
        Flip();
        Gravity();
        Animations();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * runningSpeed, rb.velocity.y);
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
        }
        else
        {
            rb.gravityScale = baseGravity;
        }

        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
            jumpCounter = extraJumps;
        }
        else
            coyoteCounter -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }else if (collision.CompareTag("Door"))
            door = collision.GetComponent<Door>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
        else if (collision.CompareTag("Door"))
            door = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer);
    }

    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));

        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void Animations()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.00001f)
        {
            if (runningSpeed != speed)
                Animator.SetFloat("xVelocity", 3f);
            else
                Animator.SetFloat("xVelocity", 2f);
        } else
            Animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

        Animator.SetFloat("yVelocity", rb.velocity.y);
        Animator.SetBool("isGrounded", IsGrounded());
        Animator.SetBool("isClimbing", isClimbing);
    }

    private void Flip()
    {
        if (horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void Walk(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (coyoteCounter <= 0 && jumpCounter <= 0) return;

            SoundManager.instance.PlaySound(jumpSound);
            Animator.SetTrigger("Jump");

            if (IsGrounded())
            {
                coyoteCounter = 0;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (coyoteCounter > 0)
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            else if (jumpCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCounter--;
            }

            coyoteCounter = 0;
            
        }
    }

    public void Climb(InputAction.CallbackContext context)
    {
        if (context.performed && isLadder)
        {
            isClimbing = true;
            vertical = context.ReadValue<Vector2>().y;
            source.clip = climbSound;
            source.Play();
        }
        else if (context.canceled)
        {
            source.Stop();
            vertical = 0.0f;
        }
    }

    public void Run (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            runningSpeed *= speedMultiplier;
        } else if (context.canceled)
        {
            runningSpeed = speed;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (door == null) return;
        if (context.performed)
        {
            door.Run();
        }

    }
}
