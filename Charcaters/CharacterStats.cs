using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public Player player;
    public bool cpu;

    public float walkspeed;
    public float runspeed;
    public bool still;

    public bool charge;
    public float burst = 1;

    public float jumpForce;
    public int jumpTotal = 1;
    public int jumpNum;
    public bool fastfall;

    public Rigidbody2D rb;
    public BoxCollider2D collider;
    public Animator anim;

    public List<GameObject> projectiles;

    public Color color;
    public Color currentColor;

    public SpriteRenderer sprite;

    public HitBox hitbox;
    public HitBox[] hitboxes;
    public HurtBox[] hurtboxes;

    public Voice voice;
    public Voice[] voices;

    public Guard guard;
    public Transform grab;

    public bool armor;
    public bool invincible;

    public CPU ai;

    public PlayerInput input;
    Inputs inputs;

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

    // Start is called before the first frame update
    void Start()
    {
        player = transform.root.GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        hitboxes = GetComponentsInChildren<HitBox>();
        hurtboxes = GetComponentsInChildren<HurtBox>();
        voices = GetComponentsInChildren<Voice>();
        grab = transform.Find("Grab");
        ai = GetComponent<CPU>();
        input = GetComponent<PlayerInput>();
        ai.dif = player.dif;
        currentColor = player.color;
        color = new Color32(0xff, 0xff, 0xff, 0xff);

        if(!player.entered)
        {
            anim.Play("Entrance");

            player.stats = GetComponent<CharacterStats>();
            player.entered = true;
            Transform c = transform;
            player.camera.players.Insert(player.player - 1, c);
            Player.allPlayers.Insert(player.player - 1, player);

            FindObjectOfType<InitializeLevel>().index++;

            if (player.cpu)
            {
                Destroy(input);
            }
        }
    }

    public LayerMask ground;

    public void End()
    {
        Destroy(gameObject);
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0, Vector2.down, 0.1f, ground);
        if(still)
        {
            if (hit.collider)
            {
                if (anim.GetBool("Launch") || anim.GetBool("Haunch"))
                {
                    if (rb.velocity.y < 0)
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * -1);
                }
            }
        }
        return hit.collider != null;
    }

    public IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.9f);
        if (!FindObjectOfType<GameManager>().go)
        {
            anim.enabled = false;
        }
        else
        {
            anim.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = color;

        if(!FindObjectOfType<GameManager>().go)
        {
            StartCoroutine(CountDown());
        }

        if (player.cpu)
        {
            cpu = true;
        }
        else
        {
            cpu = false;
        }

        Move();

        if (rb.velocity.y > 0.5f)
            anim.SetBool("air", true);

        if (IsGrounded())
        {
            if(rb.velocity.y <= 0)
            {
                jumpNum = jumpTotal;
                anim.SetBool("air", false);
            }
        }
        else
        {
            anim.SetBool("air", true);
        }

        anim.SetInteger("jumps", jumpNum);

        if (cpu)
        {
            if (ai.jump)
            {
                anim.SetBool("Jump", true);
            }
            else
            {
                anim.SetBool("Jump", false);
            }

            if (ai.attack)
            {
                anim.SetBool("Attack", true);
            }
            else
            {
                anim.SetBool("Attack", false);
            }

            if (ai.special)
            {
                anim.SetBool("Special", true);
            }
            else
            {
                anim.SetBool("Special", false);
            }

            if (ai.block)
            {
                anim.SetBool("Block", true);
            }
            else
            {
                anim.SetBool("Block", false);
            }
        }

        if(charge)
        {
            burst += Time.deltaTime;
        }
        else
        {
            burst = 1;
        }
    }


    Vector2 move;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void Move()
    {
        rb.rotation = 0;

        if (!cpu)
        {
            Vector2 directionalInput = move;

            anim.SetFloat("Horizontal", directionalInput.x);
            anim.SetFloat("Vertical", directionalInput.y);

            if (directionalInput.x >= 0.1f || directionalInput.x <= -0.1f)
            {
                StartCoroutine(Flick());
            }

            if (directionalInput.y >= 0.5f)
            {
                StartCoroutine(Upick());
            }

            if (directionalInput.y <= -0.5f)
            {
                StartCoroutine(Dlick());
            }

            if (!still)
            {
                Vector2 movementInput = new Vector2(directionalInput.x, rb.velocity.y);

                if (movementInput.x >= 0.1f || movementInput.x <= -0.1f)
                {
                    if (anim.GetFloat("Walkspeed") == 0)
                    {
                        if (anim.GetFloat("Horizontal") >= 0.1)
                        {
                            rb.velocity = new Vector2(runspeed * Time.deltaTime, movementInput.y);
                            anim.SetBool("Run", true);
                        }
                        if (anim.GetFloat("Horizontal") <= -0.1)
                        {
                            rb.velocity = new Vector2(runspeed * -1 * Time.deltaTime, movementInput.y);
                            anim.SetBool("Run", true);
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(movementInput.x * walkspeed * Time.deltaTime, movementInput.y);
                        anim.SetFloat("Walkspeed", 1f * movementInput.x);
                    }
                }
                else
                {
                    rb.velocity = new Vector2(movementInput.x * walkspeed * Time.deltaTime, movementInput.y);
                    anim.SetFloat("Walkspeed", 1f * movementInput.x);
                    anim.SetBool("Run", false);
                }

                if (IsGrounded())
                {
                    fastfall = false;

                    if (rb.velocity.y <= 0)
                    {
                        if (movementInput.x != 0)
                        {
                            transform.localScale = new Vector3(movementInput.x < 0 ? -1 : 1, 1, 1);
                            anim.SetBool("reverse", movementInput.x < 0 ? true : false);
                        }

                        if (directionalInput.y <= -1f)
                        {
                            rb.velocity = new Vector2(movementInput.x, -10);
                            fastfall = true;
                        }
                    }
                }
                else
                {
                    if (rb.velocity.y <= 0)
                    {
                        if (directionalInput.y <= -1f)
                        {
                            if(!fastfall)
                            {
                                rb.velocity *= new Vector2(movementInput.x, 5);
                                fastfall = true;
                            }
                        }
                    }
                    else
                    {
                        if (directionalInput.y <= -1f)
                        {
                            if (!fastfall)
                            {
                                rb.velocity *= new Vector2(movementInput.x, -1);
                                fastfall = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Vector2 directionalInput = new Vector2(ai.x, ai.y);

            anim.SetFloat("Horizontal", directionalInput.x);
            anim.SetFloat("Vertical", directionalInput.y);

            if (directionalInput.x >= 0.1f || directionalInput.x <= -0.1f)
            {
                StartCoroutine(Flick());
            }

            if (directionalInput.y >= 0.5f)
            {
                StartCoroutine(Upick());
            }

            if (directionalInput.y <= -0.5f)
            {
                StartCoroutine(Dlick());
            }

            if (!still)
            {
                Vector2 movementInput = new Vector2(ai.movement, rb.velocity.y);


                if (IsGrounded())
                {
                    fastfall = false;

                    if (rb.velocity.y <= 0)
                    {
                        if (movementInput.x != 0)
                        {
                            transform.localScale = new Vector3(movementInput.x < 0 ? -1 : 1, 1, 1);
                            anim.SetBool("reverse", movementInput.x < 0 ? true : false);
                        }
                    }

                    if (directionalInput.y <= -0.75f)
                    {
                        rb.velocity = new Vector2(movementInput.x, -10);
                        fastfall = true;
                    }
                }
                else
                {
                    if (rb.velocity.y <= 0)
                    {
                        if (directionalInput.y <= -0.75f)
                        {
                            if (!fastfall)
                            {
                                rb.velocity *= new Vector2(movementInput.x, 5);
                                fastfall = true;
                            }
                        }
                    }
                    else
                    {
                        if (directionalInput.y <= -0.75f)
                        {
                            if (!fastfall)
                            {
                                rb.velocity *= new Vector2(movementInput.x, -1);
                                fastfall = true;
                            }
                        }
                    }
                }

                if (movementInput.x >= 1 || movementInput.x <= -1)
                {
                    if (anim.GetFloat("Walkspeed") == 0)
                    {
                        if (movementInput.x >= 1)
                        {
                            rb.velocity = new Vector2(runspeed * Time.deltaTime, movementInput.y);
                            anim.SetBool("Run", true);
                        }
                        if (movementInput.x <= -1)
                        {
                            rb.velocity = new Vector2(runspeed * -1 * Time.deltaTime, movementInput.y);
                            anim.SetBool("Run", true);
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(movementInput.x * walkspeed * Time.deltaTime, movementInput.y);
                        anim.SetFloat("Walkspeed", 1f * movementInput.x);
                    }
                }
                else
                {
                    rb.velocity = new Vector2(movementInput.x * walkspeed * Time.deltaTime, movementInput.y);
                    anim.SetFloat("Walkspeed", 1f * movementInput.x);
                    anim.SetBool("Run", false);
                }
            }
        }
    }

    public IEnumerator Flick()
    {
        anim.SetBool("flick", true);
        yield return new WaitForSeconds(0.15f);
        anim.SetBool("flick", false);
    }

    public IEnumerator Upick()
    {
        anim.SetBool("upick", true);
        yield return new WaitForSeconds(0.15f);
        anim.SetBool("upick", false);
    }

    public IEnumerator Dlick()
    {
        anim.SetBool("dlick", true);
        yield return new WaitForSeconds(0.15f);
        anim.SetBool("dlick", false);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!cpu)
        {
            if (context.started)
            {
                anim.SetBool("Jump", true);
                anim.SetBool("air", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Jump", false);
            }

        }
    }

    public void Jump()
    {
        if (!IsGrounded())
        {
            jumpNum--;
        }
        anim.SetBool("air", true);
        rb.velocity = new Vector2(rb.velocity.y, 0);
        rb.AddForce(new Vector2(rb.velocity.y, jumpForce));
        anim.SetBool("Jump", false);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if(!cpu)
        {
            if (context.performed)
            {
                anim.SetBool("Attack", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Attack", false);
            }
        }
    }

    public void Special(InputAction.CallbackContext context)
    {
        if (!cpu)
        {
            if (context.performed)
            {
                anim.SetBool("Special", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Special", false);
            }
        }
    }

    public void Block(InputAction.CallbackContext context)
    {
        if (!cpu)
        {
            if (context.performed)
            {
                anim.SetBool("Block", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Block", false);
            }
        }
    }

    public void Grab(InputAction.CallbackContext context)
    {
        if (!cpu)
        {
            if (context.performed)
            {
                anim.SetBool("Block", true);
                anim.SetBool("Attack", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Block", false);
                anim.SetBool("Attack", false);
            }
        }
    }

    public void Taunt(InputAction.CallbackContext context)
    {
        if (!cpu)
        {
            if (context.performed)
            {
                anim.SetBool("Taunt", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Taunt", false);
            }
        }
    }

    public void OnFlick(InputAction.CallbackContext context)
    {
        if (!cpu)
        {
            if (context.performed)
            {
                StartCoroutine(Flick());
                anim.SetBool("Attack", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Attack", false);
            }
        }
    }

    public void OnUpick(InputAction.CallbackContext context)
    {
        if (!cpu)
        {
            if (context.performed)
            {
                StartCoroutine(Upick());
                anim.SetBool("Attack", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Attack", false);
            }
        }
    }

    public void OnDlick(InputAction.CallbackContext context)
    {
        if (!cpu)
        {
            if (context.performed)
            {
                StartCoroutine(Dlick());
                anim.SetBool("Attack", true);
            }

            if (context.canceled)
            {
                anim.SetBool("Attack", false);
            }
        }
    }

    public void Hit(string attack)
    {
        foreach (HitBox box in hitboxes)
            if (attack == box.attack)
            {
                hitbox = box;
                hitbox.enabled = true;
                hitbox.GetComponent<HitBox>().Whiff();
                hitbox.GetComponent<HitBox>().Attack();
            }
    }

    public void NonHit()
    {

        foreach (HitBox box in hitboxes)
        {
            hitbox = box;
            hitbox.enabled = false;
            hitbox.GetComponent<HitBox>().Attack();
            hitbox = null;
            anim.SetBool("grab", false);
            //grab.transform.parent = null;
        }
    }

    public void Voice(string type)
    {
        foreach (Voice clip in voices)
            if (type == clip.type)
            {
                voice = clip;
                voice.GetComponent<Voice>().Vocalize();
            }
    }

    public void ArmorUp()
    {
        foreach (HurtBox box in hurtboxes)
        {
            box.enabled = false;
            box.player = null;
            armor = true;
        }
    }

    public void ArmorDown()
    {
        foreach (HurtBox box in hurtboxes)
        {
            box.enabled = true;
            box.player = player;
            armor = false;
        }
    }

    public void Invincible()
    {
        foreach (HurtBox box in hurtboxes)
        {
            box.hurtbox.enabled = false;
            invincible = true;
        }
    }

    public void Vulnerable()
    {
        foreach (HurtBox box in hurtboxes)
        {
            box.hurtbox.enabled = true;
            invincible = false;
        }
    }

    public void GuardUp()
    {
        guard.enabled = true;
        guard.Block();
    }

    public void GuardDown()
    {
        guard.enabled = false;
        guard.Block();
    }

    public void Projectile(int index)
    {
        foreach(GameObject projectile in projectiles)
        {
            if(projectiles.IndexOf(projectile) == index)
            {
                Instantiate(projectiles[index], transform);
            }
        }
    }

    public void ChargeProjectile(int index)
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        if(projectiles[0] == projectiles[1])
        {
            anim.Play(state.fullPathHash, -1, 0.1f);
        }

        if (projectiles[0] == projectiles[2])
        {
            anim.Play(state.fullPathHash, -1, 0.6f);
        }

        projectiles[0] = projectiles[index];
    }

    public void Full()
    {
        anim.SetBool("charge", true);
    }

    public void Empty()
    {
        anim.SetBool("charge", false);
        projectiles[0] = null;
    }

    public void Aura(string type)
    {
        foreach (HitBox box in hitboxes)
        {
            if (type == box.attack)
            {
                hitbox = box;
                box.enabled = true;
                hitbox.GetComponent<HitBox>().Whiff();
                hitbox.GetComponent<HitBox>().Aura();
            }
        }
    }

    public void NonAura()
    {
        foreach (HitBox box in hitboxes)
        {
            hitbox = box;
            hitbox.GetComponent<HitBox>().NonAura();
            box.enabled = false;
        }
    }

    public void StartLag()
    {
        still = true;

        Vector2 directionalInput = move;

        anim.SetFloat("Horizontal", directionalInput.x);

        if (directionalInput.x != 0)
        {
            transform.localScale = new Vector3(directionalInput.x < 0 ? -1 : 1, 1, 1);
            anim.SetBool("reverse", directionalInput.x < 0 ? true : false);
        }
    }

    public void BurstX(int x)
    {
        still = true;

        Vector3 scale = transform.localScale;
        rb.velocity = new Vector2((x*burst) * scale.x, rb.velocity.y);
        charge = false;
    }

    public void BurstY(int y)
    {
        still = true;

        rb.velocity = new Vector2(rb.velocity.x, (y*burst));
        charge = false;
    }

    public void ChargeXY()
    {
        charge = true;
    }

    public void SteerX(float x)
    {
        still = true;

        Vector2 directionalInput = move;

        anim.SetFloat("Horizontal", directionalInput.x);

        x *= directionalInput.x;

        rb.velocity = new Vector2(x, rb.velocity.y);
    }

    public void SteerY(float y)
    {
        still = true;

        Vector2 directionalInput = move;

        anim.SetFloat("Vertical", directionalInput.y);

        y *= directionalInput.y;

        rb.velocity = new Vector2(rb.velocity.x, y);
    }

    public void AirLag()
    {
        if(IsGrounded())
        still = true;

        Vector2 directionalInput = move;

        anim.SetFloat("Horizontal", directionalInput.x);

        if (directionalInput.x != 0)
        {
            transform.localScale = new Vector3(directionalInput.x < 0 ? -1 : 1, 1, 1);
            anim.SetBool("reverse", directionalInput.x < 0 ? true : false);
        }
    }

    public void EndLag()
    {
        still = false;
    }

    public void PlayerColor()
    {
        color = currentColor;
    }

    public void DefualtColor()
    {
        color = new Color32(0xff, 0xff, 0xff, 0xff);
    }

/*
    public void Copy(string attack)
    {
        foreach (HitBox box in hitboxes)
            if (attack == box.attack)
            {
                hitbox = box;
                hitbox.enabled = true;
                hitbox.GetComponent<HitBox>().Whiff();
                hitbox.GetComponent<HitBox>().Copy();
            }
    }
*/

/*
    public void Data(string attack)
    {
        foreach (HitBox box in hitboxes)
            if (attack == box.attack)
            {
                hitbox = box;
                hitbox.enabled = true;
                hitbox.GetComponent<HitBox>().Whiff();
                hitbox.GetComponent<HitBox>().Data();
            }
*/
}
