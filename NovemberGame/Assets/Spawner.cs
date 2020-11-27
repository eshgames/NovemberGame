using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float min;
    [SerializeField] private float max;
    [SerializeField] private float height;
    [SerializeField] private float spawnCD;  // the spawn cooldown for 10 units. used to scale the spawn rate by
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject objTothrow;
    // Start is called before the first frame update
    void Start()
    {
        min = box.transform.position.x - box.GetComponent<SpriteRenderer>().bounds.extents.x;  // the left border's x of the box
        max = box.transform.position.x + box.GetComponent<SpriteRenderer>().bounds.extents.x;  // the right border's x of the box
        height = box.transform.position.y;  // the upper border's y of the box
        spawnCD = spawnCD / (box.transform.position.x / 10);
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        while (true)
        {
            Instantiate(objTothrow, new Vector3(Random.Range(min, max), height, 1), Quaternion.identity);  //spawning the object
            yield return new WaitForSeconds(spawnCD); 
        }
    }
}
