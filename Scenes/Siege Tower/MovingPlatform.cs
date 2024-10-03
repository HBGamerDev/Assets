using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public Transform start;
    public Transform end;
    public Transform empty;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = start.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, end.position, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, end.position) == 0)
        {
            empty = start;
            start = end;
            end = empty;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        other.transform.parent.SetParent(transform);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        other.transform.parent.SetParent(null);
    }
}
