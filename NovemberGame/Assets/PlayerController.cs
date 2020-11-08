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

    // Start is called before the first frame update
    void Start()
    {
        disFromGround = 1.3f;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    { // We need to check input in the Update function because it is getting called every frame. FixedUpdate does not and therefore can miss some inputs.
        if (Input.GetKeyDown(KeyCode.Space) && timeSinceGrounded < 0.15f)
            Jump();
        xMovement = Input.GetAxisRaw("Horizontal");  // gets a value between 0 and 1 to determine the 
    }
    private void FixedUpdate()
    {// FixedUpdate is used for physics calculations.

        rb.velocity = new Vector2(xMovement * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, disFromGround, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * disFromGround, Color.red);
        if (ray.collider == null)
        {
            timeSinceGrounded += Time.fixedDeltaTime;
        }
        else
        {
            timeSinceGrounded = 0;
        }
    }
    void Jump()
    {  //makes the player jump
        rb.velocity += Vector2.up * jumpSpeed * Time.fixedDeltaTime;
    }
}
