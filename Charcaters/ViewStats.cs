using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewStats : MonoBehaviour
{
    public GameObject list;

    public void Open()
    {
        foreach(Transform other in list.transform.parent)
        {
            other.gameObject.SetActive(false);
        }

        list.SetActive(true);
    }
}
