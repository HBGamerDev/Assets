using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public string attack;
    public int damage;
    public float kb;
    public float kbx;
    public float kby;

    public float hitbox;

    public float Bdamage;
    public float Blag;

    public CharacterStats stats;

    public Vector2 angle;

    public Collider2D hurtbox;
    public Collider2D[] hurtboxes;

    public LayerMask layer;

    public GameObject particles;

    public AudioClip[] clip;
    public AudioClip[] hit;
    public AudioSource audioSource;
    public AudioSource audioHit;

    public int effect;
    public bool aura;

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

    public void Aura()
    {
        aura = true;
    }
    public void NonAura()
    {
        aura = false;
    }


    public void Whiff()
    {
        AudioClip clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    public virtual void Attack()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, hitbox, layer);

        if (enabled)
        {
            if (particles != null)
            {
                particles.SetActive(true);
            }

            /*
            foreach (Transform child in transform)
            {
                if(child.gameObject.GetComponent<HitBox>())
                {
                    child.gameObject.GetComponent<HitBox>().enabled = true;
                    child.gameObject.GetComponent<HitBox>().Attack();
                    enabled = false;
                }
            }
            */

            foreach (Collider2D enemy in hit)
            {
                foreach (Collider2D limb in hurtboxes)
                    if (enemy == limb)
                    {
                        hurtbox = limb;

                        if (effect == 2)
                        {
                            if (transform.GetComponent<Projectile>() != null)
                            {
                                stats.anim.SetBool("grab", true);
                            }
                        }
                    }
                if (enemy != hurtbox)
                {
                    enabled = false;
                    AudioClip hitclip = GetRandomHit();
                    audioHit.PlayOneShot(hitclip);
                    audioHit.pitch = UnityEngine.Random.Range(0.9f, 1.2f);
                    if (effect == 0)
                    {
                        if(enemy.GetComponent<HurtBox>())
                        {
                            enemy.GetComponent<HurtBox>().TakeDamage(damage, kb, kbx, kby, angle, stats.transform);
                        }
                        else
                        {
                            enemy.GetComponent<Guard>().Hit(Bdamage, Blag);
                        }
                    }
                    if (effect == 1)
                    {
                        if (enemy.GetComponent<HurtBox>())
                        {
                            enemy.GetComponent<HurtBox>().TakeDamage(damage, kb, kbx, kby, angle, stats.transform);

                            if(transform.GetComponent<Projectile>() != null)
                            {
                                Instantiate(transform.GetComponent<Projectile>().pocket, transform);
                                enabled = true;
                            }
                        }
                        else
                        {
                            enemy.GetComponent<Guard>().Hit(Bdamage, Blag);
                        }
                        stats.anim.SetBool("grab", true);
                    }
                    if (effect == 2)
                    {
                        if (enemy.GetComponent<HurtBox>())
                        {
                            enemy.GetComponent<HurtBox>().TakeDamage(damage, kb, kbx, kby, angle, stats.transform);
                        }
                        else
                        {
                            enemy.GetComponent<Guard>().Hit(Bdamage, Blag);
                        }
                        stats.anim.Play("FreeFall");
                    }
                    if (effect == 3)
                    {
                        if (enemy.GetComponent<HurtBox>())
                        {
                            enemy.GetComponent<HurtBox>().Paralyze(damage, kb, kbx, kby, angle, stats.transform);
                        }
                        else
                        {
                            enemy.GetComponent<Guard>().Hit(Bdamage, Blag);
                        }
                    }
                    return;
                }
            }
        }
        else
        {
            if (particles != null)
            {
                particles.SetActive(false);
            }

            if (aura)
            {
                particles.SetActive(true);
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hitbox);
        }
    }

    public void Copy()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, hitbox, layer);

        if (enabled)
        {
            if (particles != null)
            {
                particles.SetActive(true);
            }

            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<HitBox>())
                {
                    child.gameObject.GetComponent<HitBox>().enabled = true;
                    child.gameObject.GetComponent<HitBox>().Attack();
                    enabled = false;
                }
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
                    enemy.GetComponent<HurtBox>().TakeDamage(damage, kb, kbx, kby, angle, stats.transform);
                    stats.anim.runtimeAnimatorController = enemy.gameObject.GetComponent<HurtBox>().stats.anim.runtimeAnimatorController;

                    foreach(HitBox box in stats.hitboxes)
                    {
                        for(int i = 0; i < enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes.Length; i++)
                        {
                            if(box.attack == enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].attack)
                            {
                                box.damage = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].damage;
                                box.kb = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].kb;
                                box.kbx = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].kbx;
                                box.kby = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].kby;
                                box.hitbox = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].hitbox;
                                box.angle = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].angle;
                                box.Bdamage = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].Bdamage;
                                box.Blag = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].Blag;
                                box.clip = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].clip;
                                box.hit = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].hit;
                                box.effect = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].effect;
                                box.stats.projectiles = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].stats.projectiles;

                                if (enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.parent.name == "Hand.R")
                                {
                                    box.transform.parent = box.stats.transform.Find("Torso/Shoulder.R/Arm.R/Hand.R");
                                }

                                if (enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.parent.name == "Hand.L")
                                {
                                    box.transform.parent = box.stats.transform.Find("Torso/Shoulder.L/Arm.L/Hand.L");
                                }

                                if (enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.parent.name == "Head")
                                {
                                    box.transform.parent = box.stats.transform.Find("Torso/Head");
                                }

                                if (enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.parent.name == "Mouth")
                                {
                                    box.transform.parent = box.stats.transform.Find("Torso/Head/Mouth");
                                }

                                if (enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.parent.name == "Foot.R")
                                {
                                    box.transform.parent = box.stats.transform.Find("Torso/Thigh.R/Leg.R/Foot.R");
                                }

                                if (enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.parent.name == "Foot.L")
                                {
                                    box.transform.parent = box.stats.transform.Find("Torso/Thigh.L/Leg.L/Foot.L");
                                }

                                if (enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.parent.parent.name == "Hand.R")
                                {
                                    box.transform.parent = box.stats.transform.Find("Torso/Shoulder.R/Arm.R/Hand.R");
                                }

                                if (enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.parent.name == "Wing")
                                {
                                    box.transform.parent = box.stats.transform.Find("Torso/Wing");
                                }

                                box.transform.localPosition = enemy.gameObject.GetComponent<HurtBox>().stats.hitboxes[i].transform.localPosition;
                            }
                        }
                    }
                    return;
                }
            }
        }
        else
        {
            if (particles != null)
            {
                particles.SetActive(false);
            }

            if (aura)
            {
                particles.SetActive(true);
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

    public void Data()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, hitbox, layer);

        if (enabled)
        {
            if (particles != null)
            {
                particles.SetActive(true);
            }

            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<HitBox>())
                {
                    child.gameObject.GetComponent<HitBox>().enabled = true;
                    child.gameObject.GetComponent<HitBox>().Attack();
                    enabled = false;
                }
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
                    enemy.GetComponent<HurtBox>().TakeDamage(damage, kb, kbx, kby, angle, stats.transform);

                    stats.walkspeed = enemy.gameObject.GetComponent<HurtBox>().stats.walkspeed;
                    stats.runspeed = enemy.gameObject.GetComponent<HurtBox>().stats.runspeed;
                    stats.jumpForce = enemy.gameObject.GetComponent<HurtBox>().stats.jumpForce;
                    stats.jumpTotal = enemy.gameObject.GetComponent<HurtBox>().stats.jumpTotal;
                    stats.rb.mass = enemy.gameObject.GetComponent<HurtBox>().stats.rb.mass;
                    stats.rb.gravityScale = enemy.gameObject.GetComponent<HurtBox>().stats.rb.gravityScale;


                    return;
                }
            }
        }
        else
        {
            if (particles != null)
            {
                particles.SetActive(false);
            }

            if (aura)
            {
                particles.SetActive(true);
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
}
