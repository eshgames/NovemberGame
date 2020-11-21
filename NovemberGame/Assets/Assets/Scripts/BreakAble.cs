using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAble : MonoBehaviour
{
    public float fallSpeed;
    private Rigidbody2D rb;
    private bool startBreaking;
    public float seconds;
    private BoxCollider2D collide;
    private void Awake()
    {
        collide = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        startBreaking = false;
    }
    // Update is called once per frame
    void Update()
    {

        if (startBreaking)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
            collide.enabled = false;
            gameObject.layer = 0;
        }
            
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
            StartCoroutine(Break());
    }
    IEnumerator Break()
    {
        yield return new WaitForSeconds(seconds);
        startBreaking = true;
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);

    }
}
