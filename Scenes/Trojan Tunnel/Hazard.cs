using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int damage;
    public float kb;
    public float kbx;
    public float kby;

    public Vector2 angle;

    public AudioClip[] hit;
    public AudioSource audioHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        AudioClip hitclip = GetRandomHit();
        audioHit.PlayOneShot(hitclip);
        other.gameObject.GetComponentInChildren<HurtBox>().TakeDamage(damage, kb, kbx, kby, angle, transform);
        return;
    }

    AudioClip GetRandomHit()
    {
        return hit[UnityEngine.Random.Range(0, hit.Length)];
    }
}
