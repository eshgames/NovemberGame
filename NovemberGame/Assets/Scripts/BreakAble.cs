using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAble : MonoBehaviour
{
    public float fallSpeed;
    Rigidbody2D rb;
    bool startBreaking;
    private void Awake()
    {
        fallSpeed = 5;
        rb = GetComponent<Rigidbody2D>();
        startBreaking = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(startBreaking)
            transform.Translate(Vector2.down * fallSpeed*Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Player"))
            StartCoroutine(Break());
    }
    IEnumerator Break()
    {
        yield return new WaitForSeconds(1.5f);
        startBreaking = true;
        
    }
}
