using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 2f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private float groundCheckDistance = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD or Arrow keys
        moveInput.x = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        // Ground check (raycast down)
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance);
    }

    private void FixedUpdate()
    {
        // Apply horizontal velocity
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        // Apply drag based on grounded state
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

}
