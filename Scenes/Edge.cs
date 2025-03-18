using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<CharacterStats>() && other.transform.position.y < collider.transform.position.y)
        {
            StartCoroutine(Ledge(other.gameObject.GetComponent<CharacterStats>()));
            other.transform.parent = collider.transform;
            other.transform.position = collider.transform.position;
        }
    }

    public IEnumerator Ledge(CharacterStats stat)
    {
        stat.anim.SetBool("ledge", true);
        stat.Invincible();
        yield return new WaitForSeconds(0.1f);
        stat.anim.SetBool("ledge", false);
        yield return new WaitForSeconds(0.4f);
        stat.Vulnerable();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<CharacterStats>())
        {
            other.transform.parent = other.gameObject.GetComponent<CharacterStats>().player.transform;
        }
    }
}
