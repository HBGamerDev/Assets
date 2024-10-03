using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    public BoxCollider2D bounds;

    public GameObject explosion;

    public void OnTriggerExit2D(Collider2D character)
    {
        if (character != bounds)
        {
            if(character.transform.position.y > bounds.size.y / 2)
            {
                character.GetComponent<HurtBox>().Fall();
            }
            else if(character.enabled)
            {
                if(character.gameObject.GetComponentInChildren<HurtBox>())
                {
                    Instantiate(explosion, character.transform);
                    Destroy(character.gameObject, 1f);
                    character.GetComponent<HurtBox>().Death();
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (enabled)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, bounds.size);
        }
    }
}
