using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnCursor : MonoBehaviour
{
    public GameObject prefab;
    public PlayerInput input;

    void Awake()
    {
        input = prefab.GetComponent<PlayerCursor>().input;

        var root = GameObject.Find("Player Slot Container");
        if(root != null)
        {
            var menu = Instantiate(prefab, prefab.transform);
            menu.GetComponent<PlayerCursor>().SetPlayerIndex(input.playerIndex);
        }
    }
}
