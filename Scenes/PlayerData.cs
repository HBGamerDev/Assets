using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public int player;
    public bool cpu;

    public int damage;

    public Player p;
    public Character character;

    public int score;

    // Start is called before the first frame update
    void Start()
    {
        character = p.GetComponent<Player>().character;

        Sprite render = character.render;

        string name = character.name;

        transform.Find("render").GetComponent<Image>().sprite = render;
        transform.Find("name").GetComponent<Text>().text = name;
    }

    // Update is called once per frame
    void Update()
    {
        if(p.character == null)
        {
            gameObject.SetActive(false);
        }

        if (GameManager.clones)
        {
            score = p.score;
            transform.Find("score").gameObject.SetActive(true);
            transform.Find("score").GetComponent<Text>().text = "" + score;

            if(player != 1)
            {
                gameObject.SetActive(false);
            }
        }

        character = p.GetComponent<Player>().character;

        Sprite render = character.render;

        string name = character.name;

        transform.Find("render").GetComponent<Image>().sprite = render;
        transform.Find("name").GetComponent<Text>().text = name;

        damage = p.damage;
        transform.Find("damage").GetComponent<Text>().text = "" + damage;

        if(damage < 30)
        {
            transform.Find("damage").GetComponent<Text>().color = new Color32(0xff, 0xff, 0xff, 0xff);
        }
        else
        {
            if (damage < 50)
            {
                transform.Find("damage").GetComponent<Text>().color = new Color32(0xff, 0xff, 0, 0xff);
            }
            else
            {
                if (damage < 80)
                {
                    transform.Find("damage").GetComponent<Text>().color = new Color32(0xff, 0, 0, 0xff);
                }
                else
                {
                    if (damage < 100)
                    {
                        transform.Find("damage").GetComponent<Text>().color = new Color32(0x7f, 0, 0, 0xff);
                    }
                    else
                    {
                        transform.Find("damage").GetComponent<Text>().color = new Color32(0, 0, 0, 0xff);
                    }
                }
            }
        }

        if (GameManager.versus)
        {
            score = p.score;
            transform.Find("score").gameObject.SetActive(true);
            transform.Find("score").GetComponent<Text>().text = "" + score;
        }
    }
}
