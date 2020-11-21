using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    private Rigidbody2D player; // The transform of the player's game object
    [SerializeField]
    private Vector3 offset; // The offset of the camera position from the middle of the screen
    private Camera cam;
    // Start is called before the first frame update
    private void Awake()
    {
        cam = Camera.main;
        PlayerLife.PlayerDeath += OnPlayerDeath;
    }
    void OnPlayerDeath(object source, System.EventArgs e)
    {   // Gets called when the player dies
        //The function disables the script
        Debug.Log("Camera disabled");
        this.enabled = false;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

        //transform.position = new Vector3(player.position.x, player.position.y, player.position.z - 10);

    }

    private void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 targertPos = (Vector3)player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targertPos, ref velocity, 0.01f);


    }
}
