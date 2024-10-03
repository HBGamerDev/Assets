using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public int player;

    public Transform selecter;
    public bool selecting;
    public float radius;

    public GraphicRaycaster ray;
    public PointerEventData ped = new PointerEventData(null);
    public Transform currentCharacter;

    public bool cpu;

    public Roster roster;
    public GameObject skins;

    public Player p;

    public GameManager manager;

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    void Start()
    {
       roster.ShowCharacterInSlot(player, null);

        p = roster.psc.GetChild(player - 1).GetComponent<Player>();
    }

    public IEnumerator Show()
    {
        p.difficulty.GetComponent<Dropdown>().Show();
        yield return new WaitForSeconds(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        roster = FindObjectOfType<Roster>();

        if (selecter == null)
        {
            selecter = FindObjectOfType<PlayerCursor>().transform;
        }

        float distance = Vector3.Distance(selecter.position, transform.position);

        //CONFIRM
        if (Input.GetButtonDown("Submit"))
        {
            if(distance <= radius)
            {
                if (!selecting)
                {
                    if (cpu)
                    {
                        Selecter(true);
                        roster.CancelCharacter(player, roster.characters[currentCharacter.GetSiblingIndex()]);
                        p.RemoveCharacter(player, currentCharacter.GetSiblingIndex());
                        int index = currentCharacter.GetSiblingIndex();
                        Character character = roster.characters[index];
                        character.selected = false;
                        return;
                    }
                }
            }

            if (currentCharacter != null)
            {
                if(cpu)
                {
                    if (selecting)
                    {
                        Selecter(false);
                        roster.ConfirmCharacter(player, roster.characters[currentCharacter.GetSiblingIndex()]);
                        p.AddCharacter(player, currentCharacter.GetSiblingIndex());
                        int index = currentCharacter.GetSiblingIndex();
                        Character character = roster.characters[index];
                        character.selected = true;
                        return;
                    }
                }
            }
            else
            {
                if (cpu)
                {
                    if (selecting)
                    {
                        StartCoroutine(Show());
                        return;
                    }
                }
            }

            if(roster.Ready.active)
            {
                int index = currentCharacter.GetSiblingIndex();
                Character character = roster.characters[index];
                character.selected = false;

                roster.Stage();
                return;
            }
        }

        //CANCEL

        if (Input.GetButton("Cancel"))
        {
            if (selecting)
            {
                if (!cpu)
                {
                    roster.wait += Time.deltaTime;
                    if (roster.wait >= 1)
                    {
                        manager.Menu();
                    }
                }
            }
        }
        else
        {
            roster.wait = 0;
        }

        if (selecting)
        {
            transform.position = selecter.position;
        }

        ped.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        ray.Raycast(ped, results);

        if (selecting)
        {
            if (results.Count > 0)
            {
                Transform rayC = results[0].gameObject.transform;

                if (rayC != currentCharacter)
                {
                    SetCurrentCharacter(rayC);
                }
            }
            else
            {
                if (currentCharacter != null)
                {
                    SetCurrentCharacter(null);
                }
            }
        }
        selecter = transform.parent.GetComponentInChildren<PlayerCursor>().transform;
    }



    public void SetCurrentCharacter(Transform t)
    {
        currentCharacter = t;

        if (t != null)
        {
            int index = t.GetSiblingIndex();
            Character character = roster.characters[index];
            roster.ShowCharacterInSlot(player, character);
            if (roster.characters[index].selected)
            {
                if (roster.characters[index+1].selected)
                {
                    if (roster.characters[index + 2].selected)
                    {
                        currentCharacter = roster.transform.GetChild(currentCharacter.GetSiblingIndex() + 3);
                        index = currentCharacter.GetSiblingIndex();
                        character = roster.characters[index];
                        roster.ShowCharacterInSlot(player, character);
                        p.RemoveCharacter(player, currentCharacter.GetSiblingIndex());
                        p.AddCharacter(player, currentCharacter.GetSiblingIndex());
                        return;
                    }
                    currentCharacter = roster.transform.GetChild(currentCharacter.GetSiblingIndex() + 2);
                    index = currentCharacter.GetSiblingIndex();
                    character = roster.characters[index];
                    roster.ShowCharacterInSlot(player, character);
                    p.RemoveCharacter(player, currentCharacter.GetSiblingIndex());
                    p.AddCharacter(player, currentCharacter.GetSiblingIndex());
                    return;
                }
                currentCharacter = roster.transform.GetChild(currentCharacter.GetSiblingIndex() + 1);
                index = currentCharacter.GetSiblingIndex();
                character = roster.characters[index];
                roster.ShowCharacterInSlot(player, character);
                p.RemoveCharacter(player, currentCharacter.GetSiblingIndex());
                p.AddCharacter(player, currentCharacter.GetSiblingIndex());
            }
        }
        else
        {
            roster.ShowCharacterInSlot(player, null);
        }
    }

    public void ShowSkins()
    {
        skins.SetActive(true);
        roster.ShowCharacterInSlot(player, roster.characters[currentCharacter.GetSiblingIndex()]);
    }

    public void HideSkins0()
    {
        skins.SetActive(false);
        SetCurrentCharacter(roster.transform.GetChild(currentCharacter.GetSiblingIndex()));
        p.RemoveCharacter(player, currentCharacter.GetSiblingIndex());
        p.AddCharacter(player, currentCharacter.GetSiblingIndex());
        return;
    }

    public void HideSkins1()
    {
        skins.SetActive(false);
        SetCurrentCharacter(roster.transform.GetChild(currentCharacter.GetSiblingIndex() + 1));
        p.RemoveCharacter(player, currentCharacter.GetSiblingIndex());
        p.AddCharacter(player, currentCharacter.GetSiblingIndex());
        return;
    }

    public void HideSkins2()
    {
        skins.SetActive(false);
        SetCurrentCharacter(roster.transform.GetChild(currentCharacter.GetSiblingIndex() + 2));
        p.RemoveCharacter(player, currentCharacter.GetSiblingIndex());
        p.AddCharacter(player, currentCharacter.GetSiblingIndex());
        return;
    }

    public void HideSkins3()
    {
        skins.SetActive(false);
        SetCurrentCharacter(roster.transform.GetChild(currentCharacter.GetSiblingIndex() + 3));
        p.RemoveCharacter(player, currentCharacter.GetSiblingIndex());
        p.AddCharacter(player, currentCharacter.GetSiblingIndex());
        return;
    }

    public void Selecter(bool trigger)
    {
        selecting = trigger;
    }
}
