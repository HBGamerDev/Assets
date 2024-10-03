using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBox : HitBox
{
    void Awake()
    {
        stats = transform.root.GetComponentInChildren<CharacterStats>();
        hurtboxes = transform.root.GetComponentsInChildren<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Attack();
    }

    public void Whiff()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    public override void Attack()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, hitbox, layer);

        if (enabled)
        {
            if (particles != null)
            {
                particles.SetActive(true);
            }


            foreach (Collider2D enemy in hit)
            {
                foreach (Collider2D limb in hurtboxes)
                    if (enemy == limb)
                    {
                        hurtbox = limb;
                    }
                if (enemy != hurtbox)
                {
                    enabled = false;
                    AudioClip hitclip = GetRandomHit();
                    audioHit.PlayOneShot(hitclip);
                    enemy.GetComponent<Projectile>().Reflect(damage, stats.transform);
                }
            }
        }
        else
        {
            if (particles != null)
            {
                particles.SetActive(false);
            }

            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<HitBox>())
                {
                    child.gameObject.GetComponent<HitBox>().enabled = false;
                    child.gameObject.GetComponent<HitBox>().Attack();
                }
            }
        }
    }

    AudioClip GetRandomClip()
    {
        return clip[UnityEngine.Random.Range(0, clip.Length)];
    }

    AudioClip GetRandomHit()
    {
        return hit[UnityEngine.Random.Range(0, hit.Length)];
    }

    public void OnDrawGizmos()
    {
        if (enabled)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, hitbox);
        }
    }
}
