using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public delegate void PlayerDeathEventHandler(object source, System.EventArgs e);  // the delegate for the player death event
    public static event PlayerDeathEventHandler PlayerDeath;  //Static playerdeath event because it can only occur once.
    // Start is called before the first frame update
    void Start()
    {
        PlayerDeath += OnDeath;
    }
    public static void Kill()
    {  //Invokes the playerdeath event
        PlayerDeath?.Invoke(typeof(PlayerLife),System.EventArgs.Empty);
    }
    public virtual void OnDeath(object source, System.EventArgs e)
    {   // Gets called when the player dies somehow
        // Starts the death animation of the player and destroys the gameobject of the player
        //Start death animation
        Debug.Log("Player died");
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
