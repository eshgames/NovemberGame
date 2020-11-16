using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    private Camera cam;
    // Start is called before the first frame update
    private void Awake()
    {
        cam = Camera.main;
        PlayerLife.PlayerDeath += OnPlayerDeath;
    }
    void OnPlayerDeath(object source,System.EventArgs e)
    {   // Gets called when the player dies
        //The function disables the script
        Debug.Log("Camera disabled");
        this.enabled = false;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        
        transform.position = new Vector3(player.position.x, player.position.y, player.position.z - 10);
            
    }
}
