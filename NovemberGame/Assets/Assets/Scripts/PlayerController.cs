using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;  // The gameobjec's rigidbody
    public float moveSpeed;  // The walk speed
    public float jumpSpeed;  // The jump speed
    private float timeSinceGrounded = 0;  // the time since last time the player was in ground
    public float disFromGround;  // the distance from the ground to check
    public LayerMask groundLayer;  // The layermask of the ground to classify whether you are above a ground or something else
    private float xMovement;  // the direction of the movement(between -1 and 1) alog the x axis(horizontal)
    public bool onSlider;
    bool isJumping;
    float angle;
    Vector2 normal;
    RaycastHit2D ray;

    [Header("Wall Jump")]
    [SerializeField] float wallJumpDelay = 0.2f;
    [SerializeField] float slideSpeed = 0.3f;
    [SerializeField] float wallDistance = 0.5f;
    private bool isSliding = false;
    private RaycastHit2D wallCheckHit;
    private float jumpTime;

    [Header("Misc")]
    private bool isGrounded;
    private bool isMovingRight = true;

    void Start()
    {
        onSlider = false;
        disFromGround = 0.84f;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    { // We need to check input in the Update function because it is getting called every frame. FixedUpdate does not and therefore can miss some inputs.
        ray = Physics2D.Raycast(transform.position, Vector2.down, disFromGround, groundLayer);
        if (ray.collider == null)
        {
            timeSinceGrounded += Time.fixedDeltaTime;
        }
        else
        {
            timeSinceGrounded = 0;
        }
        xMovement = Input.GetAxisRaw("Horizontal");  // gets a value between 0 and 1 to determine the
    }

    private void FixedUpdate()
    {  // FixedUpdate is used for physics calculations because it does not change with the fps the game runs on.        
       //RaycastHit2D rotRay = Physics2D.Raycast(transform.position, Vector2.down, 10, groundLayer);
       //normal = ray.normal;
       //if (!onSlider)
       //    rb.velocity = new Vector2(xMovement * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
       //else if( onSlider && ray.collider !=null)
       //{            
       //    angle = Mathf.Atan2(ray.normal.x, ray.normal.y) * Mathf.Rad2Deg;  // the angle of the slide
       //    float xV = Mathf.Cos(angle * Mathf.Deg2Rad);
       //    float yV = Mathf.Sin(angle * Mathf.Deg2Rad);
       //    rb.velocity = new Vector2( xV*moveSpeed * Time.fixedDeltaTime * Mathf.Sign(rb.velocity.x), yV*Mathf.Sign(rb.velocity.y) * moveSpeed * Time.fixedDeltaTime);

        //    //rb.AddForce(new Vector2(Mathf.Cos(angle)* rb.velocity.x * Time.deltaTime, Mathf.Sign(Mathf.Abs(angle)) * rb.velocity.y * Time.deltaTime)); // calculating the force to apply while on the slide
        //    //rb.AddForce(new Vector2(0.5f * rb.velocity.x * Time.deltaTime, 0.25f * rb.velocity.y * Time.deltaTime));
        //}

        //Debug.DrawRay(transform.position, Vector2.down * disFromGround, Color.red);
        //Debug.DrawRay(transform.position, ray.normal*disFromGround, Color.red);

        //transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, rotRay.normal));        
        CheckGround();
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            Jump();
        Flip();
        Movement();
        WallJump();
    }

    void CheckGround()
    {
        bool isTouchingGround = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, groundLayer);
        if (isTouchingGround)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    isJumping = false;
    //    if (collision.collider.tag == "Ground")
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.collider.tag.Equals("Slider"))  // do with switch
    //    {

    //        onSlider = true;
    //        transform.eulerAngles = new Vector3(0, 0, collision.collider.gameObject.transform.rotation.eulerAngles.z);  // rotating the player when on the slide
    //    }
    //    else
    //    {
    //        transform.eulerAngles = new Vector3(0, 0, 0);  // rotating back to normal rotation when not on the slide.
    //        //rb.velocity = Vector2.zero;
    //    }

    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.collider.tag.Equals("Slider"))
    //    {
    //        onSlider = false;
    //    }
    //    else if (collision.collider.tag == "Ground")
    //    {
    //        isGrounded = false;
    //    }
    //}

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

    void Jump()
    {  //makes the player jump       
        isJumping = true;
        rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        //if (onSlider)
        //    //rb.velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * (jumpSpeed)  * Mathf.Sign(rb.velocity.x), -Mathf.Sign(rb.velocity.y) * Mathf.Sin(angle * Mathf.Deg2Rad) * (jumpSpeed));
        ////rb.velocity = new Vector2(-Mathf.Sign(rb.velocity.y) * Mathf.Sin(angle * Mathf.Deg2Rad) * moveSpeed * Time.fixedDeltaTime, Mathf.Cos(angle * Mathf.Deg2Rad) * moveSpeed * Time.fixedDeltaTime * Mathf.Sign(rb.velocity.x));
        //    rb.velocity = new Vector2(normal.x * jumpSpeed , normal.y * jumpSpeed );
        //else
        //    rb.velocity += Vector2.up * jumpSpeed * Time.fixedDeltaTime;
    }
}
