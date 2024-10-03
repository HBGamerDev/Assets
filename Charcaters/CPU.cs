using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU : MonoBehaviour
{
    public float movement;
    public bool patrol;
    public bool jump;
    public bool attack;
    public bool special;
    public bool block;

    public float x;
    public float y;

    public int dif;

    public GameObject Ledge;
    public GameObject Redge;

    public float distanceL;
    public float distanceR;

    // Start is called before the first frame update
    void Start()
    {
        if(enabled)
        {
            StartCoroutine(CPUJump());
            StartCoroutine(CPUAttack());
            StartCoroutine(CPUSpecial());
            StartCoroutine(CPUBlock());

            if (patrol == false)
            {
                StartCoroutine(CPUPatrol());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ledge = GameObject.Find("Ledge.L");
        Redge = GameObject.Find("Ledge.R");

        Vector3 pos = transform.position;

        distanceL = pos.x - Ledge.transform.position.x;
        distanceR = pos.x - Redge.transform.position.x;

        if(distanceL > 0 && distanceR < 0)
        {
            if (patrol == false)
            {
                StartCoroutine(CPUPatrol());
            }
        }
        else
        {
            if (distanceL <= 0)
            {
                patrol = false;
                movement = 2f;

                y = 1f;
                jump = true;
                StartCoroutine(CPUJump());
                special = true;
                StartCoroutine(CPUSpecial());
            }

            if (distanceR >= 0)
            {
                patrol = false;
                movement = -2f;

                y = 1f;
                jump = true;
                StartCoroutine(CPUJump());
                special = true;
                StartCoroutine(CPUSpecial());
            }
        }
    }

    public IEnumerator CPUPatrol()
    {
        if(enabled)
        {
            if(distanceL < 2.5)
            {
                if (dif == 0)
                {
                    movement = Random.Range(-0.1f, 1f);
                    int pause = Random.Range(1, 2);

                    patrol = true;
                    yield return new WaitForSeconds(pause);
                    movement = 0;
                    yield return new WaitForSeconds(pause);
                    patrol = false;
                }

                if (dif == 1)
                {
                    movement = Random.Range(-0.2f, 2f);
                    x = Random.Range(-1f, 1f);
                    y = Random.Range(-1f, 1f);
                    int pause = Random.Range(0, 2);

                    patrol = true;
                    yield return new WaitForSeconds(pause);
                    movement = 0;
                    patrol = false;
                }
            }

            if (distanceR > -2.5)
            {
                if (dif == 0)
                {
                    movement = Random.Range(-1f, 0.1f);
                    int pause = Random.Range(1, 2);

                    patrol = true;
                    yield return new WaitForSeconds(pause);
                    movement = 0;
                    yield return new WaitForSeconds(pause);
                    patrol = false;
                }

                if (dif == 1)
                {
                    movement = Random.Range(-2f, 0.2f);
                    x = Random.Range(-1f, 0.1f);
                    y = Random.Range(-1f, 0.1f);
                    int pause = Random.Range(0, 2);

                    patrol = true;
                    yield return new WaitForSeconds(pause);
                    movement = 0;
                    patrol = false;
                }
            }
        }
    }

    public IEnumerator CPUJump()
    {
        if(enabled)
        {
            if(jump)
            {
                yield return new WaitForSeconds(0.1f);
                jump = false;
            }

            if(dif == 0)
            {
                int pause = Random.Range(0, 10);

                yield return new WaitForSeconds(pause);
                jump = true;
                yield return new WaitForSeconds(0.1f);
                jump = false;
                StartCoroutine(CPUJump());
            }

            if (dif == 1)
            {
                int pause = Random.Range(0, 5);

                yield return new WaitForSeconds(pause);
                jump = true;
                yield return new WaitForSeconds(0.1f);
                jump = false;
                StartCoroutine(CPUJump());
            }
        }
        else
        {
            jump = false;
        }
    }

    public IEnumerator CPUAttack()
    {
        if(enabled)
        {
            if(dif == 0)
            {
                int pause = Random.Range(0, 5);

                yield return new WaitForSeconds(pause);
                if (movement == 0)
                {
                    attack = true;
                    yield return new WaitForSeconds(0.1f);
                    attack = false;
                }
                StartCoroutine(CPUAttack());
            }

            if (dif == 1)
            {
                int pause = Random.Range(0, 2);

                yield return new WaitForSeconds(pause);
                attack = true;
                yield return new WaitForSeconds(0.1f);
                attack = false;
                StartCoroutine(CPUAttack());
            }
        }
        else
        {
            attack = false;
        }
    }

    public IEnumerator CPUSpecial()
    {
        if (enabled)
        {
            if (special)
            {
                yield return new WaitForSeconds(0.1f);
                special = false;
            }

            if (dif == 0)
            {
                int pause = Random.Range(0, 5);

                yield return new WaitForSeconds(pause);
                if (movement == 0)
                {
                    special = true;
                    yield return new WaitForSeconds(0.1f);
                    special = false;
                }
                StartCoroutine(CPUSpecial());
            }

            if (dif == 1)
            {
                int pause = Random.Range(0, 2);

                yield return new WaitForSeconds(pause);
                special = true;
                yield return new WaitForSeconds(0.1f);
                special = false;
                StartCoroutine(CPUSpecial());
            }
        }
        else
        {
            special = false;
        }
    }

    public IEnumerator CPUBlock()
    {
        if (enabled)
        {
            if (dif == 1)
            {
                int pause = Random.Range(0, 10);

                yield return new WaitForSeconds(pause);
                block = true;
                yield return new WaitForSeconds(pause);
                block = false;
                StartCoroutine(CPUBlock());
            }
        }
        else
        {
            block = false;
        }
    }
}
