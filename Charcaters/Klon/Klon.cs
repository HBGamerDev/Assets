using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Klon : MonoBehaviour
{
    public CharacterStats stats;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (HurtBox box in stats.hurtboxes)
        {

            if (transform.parent != transform.root && transform.parent.name != "Grab")
            {
                if(box.anim.GetBool("Lit") || box.anim.GetBool("Hit") || box.anim.GetBool("Launch") || box.anim.GetBool("Haunch"))
                {
                    stats.End();
                    box.anim.SetBool("Lit", false);
                    box.anim.SetBool("Hit", false);
                    box.anim.SetBool("Launch", false);
                    box.anim.SetBool("Haunch", false);
                }
            }
        }

    }
}