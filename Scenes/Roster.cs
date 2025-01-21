using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roster : MonoBehaviour
{
    public List<Character> characters = new List<Character>();

    public GameObject cellPrefab;
    public Transform psc;

    public int ready;
    public GameObject Ready;
    public GameObject Back;
    public SpriteRenderer Backsprite;
    public float wait;
    public GameObject RuleSet;
    public AddPlayer add;

    public AudioSource sound;
    public GameObject stage;

    void Start()
    {
        foreach (Character character in characters)
        {
            SpawnCharacterCell(character);
            character.selected = false;
        }

        if(GameManager.solo == true)
        {
            if(GameManager.training == false)
            {
                ready = 3;
            }
        }
    }

    void Update()
    {
        Backsprite.size = new Vector2(wait, 1);

        if (GameManager.versus == true && GameManager.tourney == false)
        {
            RuleSet.SetActive(true);
            add = psc.GetComponent<MultiplayerManager>().add;
        }
    }

    private void SpawnCharacterCell(Character character)
    {
        GameObject charCell = Instantiate(cellPrefab, transform);

        charCell.name = character.name;

        Image render = charCell.transform.Find("render").GetComponent<Image>();
        Text name = charCell.transform.Find("name").GetComponent<Text>();

        render.sprite = character.render;
        name.text = character.name;

        if (character.alt == true)
        {
            charCell.SetActive(false);
        }
    }

    public void ShowCharacterInSlot(int player, Character character)
    {
        bool nullChar = (character == null);

        Sprite render = nullChar ? null : character.render;

        string name = nullChar ? string.Empty : character.name;

        Sprite stock = nullChar ? null : character.stock;


        Transform slot = psc.GetChild(player -1);

        slot.Find("render").GetComponent<Image>().sprite = render;
        slot.Find("name").GetComponent<Text>().text = name;

        if(slot.Find("skins").gameObject.activeInHierarchy == true)
        {
            foreach (Transform skin in slot.Find("skins"))
            {
                int index = characters.IndexOf(character);

                if (skin.GetSiblingIndex() == 0)
                {
                    skin.GetComponent<Image>().sprite = stock;
                }

                if (skin.GetSiblingIndex() == 1)
                {
                    skin.GetComponent<Image>().sprite = characters[index + 1].stock;
                }

                if (skin.GetSiblingIndex() == 2)
                {
                    skin.GetComponent<Image>().sprite = characters[index + 2].stock;
                }

                if (skin.GetSiblingIndex() == 3)
                {
                    skin.GetComponent<Image>().sprite = characters[index + 3].stock;
                }
            }
        }
    }

    public void ConfirmCharacter(int player, Character character)
    {
        ready++;
        sound.PlayOneShot(character.clip);

        if(ready == psc.childCount)
        {
            StartCoroutine(ReadyToFight());
        }
    }

    public IEnumerator ReadyToFight()
    {
        yield return new WaitForSeconds(0.1f);
        Ready.SetActive(true);
    }

    public void Stage()
    {
        stage.SetActive(true);
        StartCoroutine(ToStages());
    }

    public IEnumerator ToStages()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
        psc.gameObject.SetActive(false);
        Ready.SetActive(false);
        Back.SetActive(false);
        RuleSet.SetActive(false);
        add.gameObject.SetActive(false);
    }

    public void CancelCharacter(int player, Character character)
    {
        if(character != null)
        {
            if (ready == psc.childCount)
            {
                Ready.SetActive(false);
            }
            ready--;
        }
    }

}
