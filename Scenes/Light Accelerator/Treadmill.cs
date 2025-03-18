using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    public float speed;
    public Transform empty;

    // Update is called once per frame
    void Update()
    {
        empty.position += new Vector3 (-1, 0, 0) * (Time.deltaTime * speed);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        other.transform.SetParent(empty);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        other.transform.SetParent(other.transform.GetComponent<CharacterStats>().player.transform);
    }
}
