using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPlatform : MonoBehaviour
{
    public float weight;
    public WeightPlatform balancer;
    public Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, (-weight * 0.05f), 0);

        if(weight == 0)
        {
            transform.position = start;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        other.transform.parent.SetParent(transform);
        weight += other.transform.GetComponent<CharacterStats>().rb.mass;
        balancer.weight = -weight;
    }

    void OnCollisionExit2D(Collision2D other)
    {
        other.transform.parent.SetParent(null);
        weight -= other.transform.GetComponent<CharacterStats>().rb.mass;
        balancer.weight = -weight;
    }
}
