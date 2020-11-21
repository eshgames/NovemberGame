using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] GameObject groundCheck;
    [SerializeField] float groundRadius = 0.2f;
    private float xMovement;
    private bool isGrounded;
    private bool isMovingRight = true;

    [Header("Wall Jump")]
    [SerializeField] float wallJumpDelay = 0.2f;
    [SerializeField] float slideSpeed = 0.3f;
    [SerializeField] float wallDistance = 0.5f;
    private bool isSliding = false;
    private RaycastHit2D wallCheckHit;
    private float jumpTime;

    [Header("Misc")]
    [SerializeField] LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || isSliding && Input.GetButtonDown("Jump"))
            Jump();
    }

    private void FixedUpdate()
    {
        xMovement = Input.GetAxis("Horizontal");
        CheckGround();        
        Flip();
        Movement();
        WallJump();
    }

    void CheckGround()
    {
        bool isTouchingGround = Physics2D.OverlapCircle(groundCheck.transform.position, groundRadius, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down, Color.blue);
        if (isTouchingGround)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    void Flip()
    {
        if (Input.GetKey(KeyCode.A) && isMovingRight)
        {
            transform.Rotate(0, 180, 0);
            isMovingRight = false;
        }

        else if (Input.GetKey(KeyCode.D) && !isMovingRight)
        {
            transform.Rotate(0, 180, 0);
            isMovingRight = true;
        }
    }

    void Movement()
    {
        rb.velocity = new Vector2(xMovement * moveSpeed, rb.velocity.y);
    }

    void WallJump()
    {
        if (isMovingRight)
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
        }
        else
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
        }

        if (wallCheckHit && !isGrounded && xMovement != 0)
        {
            isSliding = true;
            jumpTime = Time.time + wallJumpDelay;
        }

        else if (jumpTime < Time.time)
        {
            isSliding = false;
        }

        if (isSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, slideSpeed, float.MaxValue));
        }
    }
}
