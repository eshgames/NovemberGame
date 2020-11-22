using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float min;
    [SerializeField] private float max;
    [SerializeField] private float height;

    [SerializeField] private GameObject objTothrow;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    IEnumerator Spawn()
    {
        while (true)
        {
            Instantiate(objTothrow, new Vector3(Random.Range(min, max), height, 1), Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
