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

    private RaycastHit2D rotRay;  //A raycast to handle the player's rotations
    private Vector2 normal;
    private float angle;
    private bool onSlider;
    bool jump;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rotRay = Physics2D.Raycast(transform.position, Vector2.down, 10, groundLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || isSliding && Input.GetButtonDown("Jump"))
            jump = true;
        
    }

    private void FixedUpdate()
    {
        xMovement = Input.GetAxis("Horizontal");
        rotRay = Physics2D.Raycast(transform.position, Vector2.down, 10, groundLayer);
        normal = rotRay.normal;
        angle = Mathf.Atan2(rotRay.normal.x, rotRay.normal.y) * Mathf.Rad2Deg;  // The angle of the plain the player currently stands on
        CheckGround();        
        Flip();
        Movement();
        if (jump)
            Jump();
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
        jump = false;
        if(!onSlider)
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        else
        {
            rb.velocity = jumpSpeed * (new Vector2(normal.x, normal.y));
        }

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
        if(!onSlider)
            rb.velocity = new Vector2(xMovement * moveSpeed, rb.velocity.y);
        else if(onSlider)
        {

            // the angle of the slide
            float xV = Mathf.Cos(angle * Mathf.Deg2Rad);
            float yV = Mathf.Sin(angle * Mathf.Deg2Rad);
            //rb.velocity = new Vector2( 0, 0) + new Vector2( moveSpeed ,   Mathf.Sign(rb.velocity.y) * moveSpeed );
            rb.AddForce(new Vector2( xV *moveSpeed, Mathf.Sign(rb.velocity.y) * moveSpeed * yV));
        }
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
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Slider"))
        {
            onSlider = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag.Equals("Slider"))
            onSlider = true;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.collider.tag.Equals("Slider"))  // do with switch
        {
            onSlider = true;
            if (rotRay.collider != null)
                transform.eulerAngles = Mathf.Sign(rotRay.collider.transform.rotation.z) * Vector3.forward * angle;
            //transform.eulerAngles = new Vector3(0, 0, collision.collider.gameObject.transform.rotation.eulerAngles.z);  // rotating the player when on the slide
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);  // rotating back to normal rotation when not on the slide.
            //rb.velocity = Vector2.zero;
        }

    }
}
