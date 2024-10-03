using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SteerProjectile : Projectile
{
    float x;
    float y;

    public PlayerInput input;
    Inputs inputs;

    Vector2 move;

    void OnEnable()
    {
        inputs.Enable();
    }

    void OnDisable()
    {
        inputs.Disable();
    }

    void Awake()
    {
        inputs = new Inputs();
    }

    void Start()
    {
        player = transform.root.GetComponent<Player>();
        boxes = GetComponent<HitBox>();
        rb = GetComponent<Rigidbody2D>();
        input = transform.parent.GetComponent<CharacterStats>().input;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        x = speedx * player.stats.anim.GetFloat("Horizontal");

        rb.velocity = new Vector2(x, rb.velocity.y);

        y =  speedy * player.stats.anim.GetFloat("Vertical");

        rb.velocity = new Vector2(rb.velocity.x, y);

        time -= Time.deltaTime;

        if (!boxes.enabled)
        {
            StartCoroutine(Free());
        }
        if (time <= 0)
        {
            Destroy(gameObject);
        }
        StartCoroutine(Free());
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
