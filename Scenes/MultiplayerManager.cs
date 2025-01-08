using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class MultiplayerManager : MonoBehaviour
{
    public List<Configure> configures;

    public SpawnCursor player;

    public int playernum;

    public PlayerInputManager manager;

    public List<GameObject> players = new List<GameObject>();
    public static List<InputDevice> devices = new List<InputDevice>();

    public static MultiplayerManager Instance { get; private set; }

    public Roster roster;
    public StageCursor cursor;

    public AddPlayer add;

    void Start()
    {
        Instance = this;
        player.prefab = players[playernum];
        playernum = 0;

        if (GameManager.versus == false || GameManager.tourney == true)
        {
            add.gameObject.SetActive(false);

            if (GameManager.tourneyPlayers == 6)
            {
                add.Add();
            }
        }
    }

    void Update()
    {
        if(player != null)
        player.prefab = players[playernum];

        if (GameManager.solo == true)
        {
            players.RemoveRange(1, players.Count - 1);

            if(GameManager.training == false)
            {
                transform.GetChild(1).gameObject.SetActive(false);
            }
        }


        player.prefab = players[playernum];
        devices[playernum] = players[playernum].GetComponent<PlayerCursor>().input.devices[0];
    }

    public List<Configure> GetConfigures()
    {
        return configures;
    }

    public void PlayerJoin(PlayerInput input)
    {
        if (playernum >= 2)
        {
            add.Add();
        }

        input.transform.SetParent(transform.GetChild(playernum));
        if (GameManager.solo == false)
        {
            playernum++;
        }
    }
}

public class Configure
{
    public Configure(PlayerInput player)
    {
        index = player.playerIndex;

        input = player;
    }

    public PlayerInput input { get; set; }

    public int index { get; set; }
}
