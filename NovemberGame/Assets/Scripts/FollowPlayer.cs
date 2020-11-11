using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        //if (Camera.main.WorldToViewportPoint(background.GetComponent<SpriteRenderer>().bounds.extents).x > 1)
        transform.position = new Vector3(player.transform.position.x - offset.x, transform.position.y, transform.position.z);// the intial position along the x axis
    }
}
