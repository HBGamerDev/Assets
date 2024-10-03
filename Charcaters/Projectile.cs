using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Player player;
    public HitBox boxes;

    public Rigidbody2D rb;

    public float speedx;
    public float speedy;

    public float time;

    public GameObject pocket;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.root.GetComponent<Player>();
        boxes = GetComponent<HitBox>();
        boxes.hurtboxes = transform.root.GetComponentsInChildren<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;

        if (!boxes.enabled)
        {
            StartCoroutine(Free());
        }
        if (time <= 0)
        {
            Destroy(gameObject);
        }

        if (transform.parent.localScale.x == -1)
        {
            Vector3 scale = transform.localScale;

            transform.localScale = new Vector3(scale.x * -1, scale.y * 1, scale.z * 1);
            rb.velocity = new Vector2(-1 * speedx * Time.deltaTime, speedy * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector2(speedx * Time.deltaTime, speedy * Time.deltaTime);
        }
        StartCoroutine(Free());
    }

    public void Reflect(int damage, Transform t)
    {
        boxes.enabled = false;
        time *= 2;
        rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y * -1);
        boxes.hurtboxes = t.root.GetComponentsInChildren<CapsuleCollider2D>();
        boxes.damage *= damage;
        boxes.damage /= 10;
        boxes.enabled = true;
    }

    public IEnumerator Free()
    {
        yield return new WaitForSeconds(0f);
            transform.parent = null;

        if (!boxes.enabled)
        {
            yield return new WaitForSeconds(0.25f);
            time = 0;
        }
    }
}
