using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    [SerializeField]
    private float min;
    // Start is called before the first frame update
    void Start()
    {
        //PlayerLife.PlayerDeath += OnHitPlayer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.down * 3 * Time.deltaTime);
        if (transform.position.y <= min)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            PlayerLife.Kill();
            Destroy(gameObject);
        }
            
    }
    
}
