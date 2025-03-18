using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard : MonoBehaviour
{
    public CharacterStats stats;
    public SpriteRenderer render;
    public Color color;

    public CapsuleCollider2D collider;

    public float health;

    public int escape;

    // Start is called before the first frame update
    void Start()
    {
        stats = transform.root.GetComponentInChildren<CharacterStats>();
        color = transform.parent.GetComponent<CharacterStats>().player.color;
        render.color = color;
        StartCoroutine(Regenerate());
        enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(enabled)
        {
            health -= Time.deltaTime / 2.5f;

            render.color = new Color(color.r, color.g, color.b, health);

            if(health <= 0)
            {
                stats.anim.speed = 1;
                transform.parent.GetComponent<CharacterStats>().anim.SetBool("crash", true);
                enabled = false;
                Block();
            }
        }
    }

    public void Hit(float damage, float lag)
    {
        health -= damage;
        
        if(health <= 0)
        {
            stats.anim.speed = 1;
            transform.parent.GetComponent<CharacterStats>().anim.SetBool("crash", true);
            enabled = false;
            Block();
            return;
        }

        if(lag > 0)
        {
            stats.anim.Play("Blockstun");
            stats.anim.speed /= lag;
        }
    }

    public void Grabbed(Transform t, int k)
    {
        enabled = false;
        Block();
        stats.anim.SetBool("Launch", true);
        escape = k * 100;
        if (k == 0)
        {
            escape = stats.player.damage;
        }
        StartCoroutine(Follow(t, k));
    }

    public void Block()
    {
        if (enabled)
        {
            render.enabled = true;
            collider.size = new Vector2(1, 1);
        }
        else
        {
            render.enabled = false;
            collider.size = new Vector2(0, 0);
        }
    }

    public IEnumerator Regenerate()
    {
        if(health >= 1)
        {
            health = 1;
            transform.parent.GetComponent<CharacterStats>().anim.SetBool("crash", false);
        }

        yield return new WaitForSeconds(0.2f);
        health += 0.04f;
        StartCoroutine(Regenerate());
    }

    public IEnumerator Follow(Transform t, int k)
    {
        if (stats.anim.GetBool("Launch") == true)
        {
            yield return new WaitForSeconds(0.01f);
            escape--;
            stats.transform.localPosition = new Vector3(0, 0, 0);
            if (escape >= 0)
            {
                stats.transform.position = t.position;
                StartCoroutine(Follow(t, k));
            }
            else
            {
                stats.anim.SetBool("Launch", false);
                stats.transform.parent = stats.player.transform;

            }
        }
        if (stats.anim.GetBool("Launch") == false)
        {
            yield return new WaitForSeconds(0.01f);
            stats.transform.position = t.position;
        }
    }
}
