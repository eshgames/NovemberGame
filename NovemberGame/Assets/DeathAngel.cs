using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAngel : MonoBehaviour
{
    public Transform player;
    public float speed;
    private float distanceFromPlayer;
   
    

    // Start is called before the first frame update
    void Start()
    {
        PlayerLife.PlayerDeath += OnPlayerCatch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed*Time.fixedDeltaTime);
        distanceFromPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceFromPlayer <= 2)
        {
            //Debug.Log("kill")
            PlayerLife.Kill();
        }   
    }
    public void OnPlayerCatch(object source, System.EventArgs e)
    {   //Gets called when the player dies
        //Starts the kill animation of the death angel and disables the script
        //Activate kill animation
        Debug.Log("kill");
        this.enabled = false;
    }
}
