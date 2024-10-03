using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public Text text;
    public Button quit;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Player p in Player.allPlayers)
        {
            if (p.player != Player.allPlayers[0].player)
            {
                if (Player.allPlayers[0].score == p.score)
                {
                    Player.allPlayers[0].stats.anim.Play("Idle");
                    text.text = "Sudden Death!";
                }
            }

            if (p.player != Player.allPlayers[1].player)
            {
                if (Player.allPlayers[1].score == p.score)
                {
                    Player.allPlayers[1].stats.anim.Play("Idle");
                    text.text = "Sudden Death!";
                }
            }

            if (p.player != Player.allPlayers[2].player)
            {
                if (Player.allPlayers[2].score == p.score)
                {
                    Player.allPlayers[2].stats.anim.Play("Idle");
                    text.text = "Sudden Death!";
                }
            }

            if (p.player != Player.allPlayers[3].player)
            {
                if (Player.allPlayers[3].score == p.score)
                {
                    Player.allPlayers[3].stats.anim.Play("Idle");
                    text.text = "Sudden Death!";
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
