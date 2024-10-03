using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitboxVisual : MonoBehaviour
{
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(transform.parent.GetComponent<HitBox>().hitbox * 2, transform.parent.GetComponent<HitBox>().hitbox * 2, 1);

        if (transform.parent.GetComponent<GrabBox>())
        {
            sprite.color = new Color32(0xff, 0, 0xff, 0xff);
        }

        if (transform.parent.GetComponent<ReflectBox>())
        {
            sprite.color = new Color32(0, 0xff, 0xff, 0xff);
        }

        if (transform.parent.GetComponent<AbsorbBox>())
        {
            sprite.color = new Color32(0, 0, 0xff, 0xff);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!transform.parent.GetComponent<HitBox>().enabled)
        {
            Destroy(gameObject);
        }
    }
}
