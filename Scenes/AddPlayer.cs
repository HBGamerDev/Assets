using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPlayer : MonoBehaviour
{
    public Transform psc;
    public Roster roster;
    public int player;
    public GameObject Addnew;

    public void Add()
    {
        roster.ready--;
        Transform slot = psc.GetChild(player - 1);
        slot.gameObject.SetActive(true);
        Destroy(gameObject);
        Addnew.SetActive(true);
        psc.gameObject.GetComponent<MultiplayerManager>().add = Addnew.GetComponent<AddPlayer>();
    }
}
