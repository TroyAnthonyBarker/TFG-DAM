using UnityEngine;
using UnityEngine.InputSystem;

public class BobMundoMovement : MonoBehaviour
{
    // References
    private Rigidbody2D rb;
    private Animator Animator;
    private AudioSource source;
    private Door door;

    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    private float horizontal;
    private float vertical;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Animations();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.CompareTag("Door"))
        {
            door = collision.GetComponent<Door>();
            Debug.Log("Is door");
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Door"))
            door = null;
    }

    private void Animations()
    {
        Animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        Animator.SetFloat("yVelocity", Mathf.Abs(rb.velocity.y));
    }

    private void Flip()
    {
        if (horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void Walk(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
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
