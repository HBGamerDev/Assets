using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public CapsuleCollider2D hurtbox;
    public float horizontalPosition;
    public float verticalPosition;

    public Player player;

    public CharacterStats stats;

    public Animator anim;

    public int escape;

    void Start()
    {
        hurtbox = GetComponent<CapsuleCollider2D>();

        player = transform.root.GetComponent<Player>();

        stats = transform.root.GetComponentInChildren<CharacterStats>();

        anim = transform.root.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(!stats.still)
        {
            anim.speed = 1;
        }
    }

    public void TakeDamage(int damage, float kb, float kbx, float kby, Vector2 angle, Transform t)
    {
        if (FindObjectOfType<ComboCounter>() != null)
        {
            if (!stats.still)
            {
                FindObjectOfType<ComboCounter>().Reset();
            }
            FindObjectOfType<ComboCounter>().Hit(damage);
        }

        float kbs = (kb + player.damage * (kbx + kby)) / stats.rb.mass;
        if (kbs < 1)
        {
            anim.SetBool("Lit", true);
            anim.SetFloat("horizontalHit", horizontalPosition);
            anim.SetFloat("verticalHit", verticalPosition);
        }
        else
        if (kbs < 4)
        {
            anim.SetBool("Hit", true);
            anim.SetFloat("horizontalHit", horizontalPosition);
            anim.SetFloat("verticalHit", verticalPosition);
        }
        else
        if (kbs < 9)
        {
            anim.SetBool("Launch", true);
            anim.SetFloat("horizontalHit", horizontalPosition);
            anim.SetFloat("verticalHit", verticalPosition);
        }
        else
        {
            anim.SetBool("Haunch", true);
            anim.SetFloat("horizontalHit", horizontalPosition);
            anim.SetFloat("verticalHit", verticalPosition);
        }

        print(kbs);

        StartCoroutine(KnockBack(kb, kbx, kby, angle, t));
        StartCoroutine(Damaged());
        player.damage += damage;

        player.hit = t.root.GetComponent<Player>();
    }

    public void Grabbed(Transform t, int k)
    {
        anim.SetBool("Hit", true);
        escape = k * 100;
        stats.transform.parent = t;
        if (k == 0)
        {
            escape = player.damage;
        }
        StartCoroutine(Follow(t, k));

        if(FindObjectOfType<ComboCounter>())
        FindObjectOfType<ComboCounter>().Reset();
    }
    public IEnumerator Follow(Transform t, int k)
    {
        if(anim.GetBool("Hit") == true)
        {
            yield return new WaitForSeconds(0.01f);
            escape--;
            stats.transform.localPosition = new Vector3(0, 0, 0);
            if(escape >= 0)
            {
                StartCoroutine(Follow(t, k));
            }
            else
            {
                anim.SetBool("Hit", false);
                stats.transform.parent = stats.player.transform;
            }
        }
        if (anim.GetBool("Hit") == false)
        {
            yield return new WaitForSeconds(0.01f);
            stats.transform.position = t.position;
        }
    }

        public IEnumerator KnockBack(float kb, float kbx, float kby, Vector2 angle, Transform t)
    {
        yield return new WaitForSeconds(0.05f);
        var side = t.position - stats.transform.position;

        if (side.x > 0)
        {
            angle *= new Vector2(-1, 1);
            kbx *= -1;
        }

        if(angle.y < 0)
        {
            kby *= -1;
        }

        var dir = angle;

        stats.rb.velocity = dir.normalized * kb / stats.rb.mass + new Vector2(player.damage * kbx, player.damage * kby);
    }

    public void Paralyze(int damage, float kb, float kbx, float kby, Vector2 angle, Transform t)
    {
        anim.speed = kb;

        player.damage += damage;
        anim.Play("Paralyze");

        if (FindObjectOfType<ComboCounter>() != null)
        {
            if (!stats.still)
            {
                FindObjectOfType<ComboCounter>().Reset();
            }
            FindObjectOfType<ComboCounter>().Hit(damage);
        }
    }

    public IEnumerator Damaged()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("Lit", false);
        anim.SetBool("Hit", false);
        anim.SetBool("Launch", false);
        anim.SetBool("Haunch", false);
        stats.transform.parent = stats.player.transform;
    }

    public void Death()
    {
        anim.SetBool("death", true);
        StartCoroutine(Dead());
    }

    public void Fall()
    {
        anim.SetBool("fall", true);
        StartCoroutine(Dead());
    }

    public IEnumerator Dead()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("death", false);
        anim.SetBool("fall", false);
    }
}
