using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerCursor : MonoBehaviour
{
    public int player;
    public float speed;

    public PlayerInput input;
    public InputDevice device;

    Inputs inputs;

    public CharacterSelect selecter;

    void OnEnable()
    {
        inputs.Enable();
    }

    void OnDisable()
    {
        inputs.Disable();
    }

    void Awake()
    {
        inputs = new Inputs();
        selecter = transform.parent.GetComponentInChildren<CharacterSelect>();
        selecter.cpu = false;
        selecter.selecting = true;
    }

    Vector2 movementInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void Select(InputAction.CallbackContext context)
    {
        float distance = Vector3.Distance(selecter.transform.position, transform.position);

        if (distance <= selecter.radius)
        {
            if (selecter.currentCharacter != null)
            {
                if (selecter.selecting)
                {
                    selecter.Selecter(false);
                    selecter.roster.ConfirmCharacter(player, selecter.roster.characters[selecter.currentCharacter.GetSiblingIndex()]);
                    selecter.p.AddCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                    int index = selecter.currentCharacter.GetSiblingIndex();
                    Character character = selecter.roster.characters[index];
                    character.selected = true;
                    return;
                }
            }
        }

    }

    public void Cancel(InputAction.CallbackContext context)
    {
        if (!selecter.selecting)
        {
            if (!selecter.cpu)
            {
                selecter.Selecter(true);
                selecter.roster.CancelCharacter(player, selecter.roster.characters[selecter.currentCharacter.GetSiblingIndex()]);
                selecter.p.RemoveCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                int index = selecter.currentCharacter.GetSiblingIndex();
                Character character = selecter.roster.characters[index];
                character.selected = false;
            }
        }

    }

    public void Skin(InputAction.CallbackContext context)
    {
        int index = selecter.currentCharacter.GetSiblingIndex();
        Character character = selecter.roster.characters[index];

        if (context.started)
        {
            if (selecter.roster.characters[index + 1].selected)
            {
                if (selecter.roster.characters[index + 2].selected)
                {
                    if (selecter.roster.characters[index + 3].selected)
                    {
                        character.selected = false;
                        index = selecter.currentCharacter.GetSiblingIndex();
                        character = selecter.roster.characters[index];
                        selecter.roster.ShowCharacterInSlot(player, character);
                        selecter.p.RemoveCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                        selecter.p.AddCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                        character.selected = true;
                        return;
                    }
                    character.selected = false;
                    selecter.currentCharacter = selecter.roster.transform.GetChild(selecter.currentCharacter.GetSiblingIndex() + 3);
                    index = selecter.currentCharacter.GetSiblingIndex();
                    character = selecter.roster.characters[index];
                    selecter.roster.ShowCharacterInSlot(player, character);
                    selecter.p.RemoveCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                    selecter.p.AddCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                    character.selected = true;
                    return;
                }
                character.selected = false;
                selecter.currentCharacter = selecter.roster.transform.GetChild(selecter.currentCharacter.GetSiblingIndex() + 2);
                index = selecter.currentCharacter.GetSiblingIndex();
                character = selecter.roster.characters[index];
                selecter.roster.ShowCharacterInSlot(player, character);
                selecter.p.RemoveCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                selecter.p.AddCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                character.selected = true;
                return;
            }

            character.selected = false;
            selecter.currentCharacter = selecter.roster.transform.GetChild(selecter.currentCharacter.GetSiblingIndex() + 1);
            index = selecter.currentCharacter.GetSiblingIndex();
            character = selecter.roster.characters[index];
            selecter.roster.ShowCharacterInSlot(player, character);
            selecter.p.RemoveCharacter(player, selecter.currentCharacter.GetSiblingIndex());
            selecter.p.AddCharacter(player, selecter.currentCharacter.GetSiblingIndex());
            character.selected = true;

            if (selecter.roster.characters[index].alt == false)
            {
                character.selected = false;
                selecter.currentCharacter = selecter.roster.transform.GetChild(selecter.currentCharacter.GetSiblingIndex() - 4);
                index = selecter.currentCharacter.GetSiblingIndex();
                character = selecter.roster.characters[index];
                selecter.roster.ShowCharacterInSlot(player, character);
                selecter.p.RemoveCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                selecter.p.AddCharacter(player, selecter.currentCharacter.GetSiblingIndex());
                character.selected = true;
            }

            return;
        }
    }

    public void SetPlayerIndex(int p)
    {
        p = player - 1;
    }

    void Update()
    {
        Vector2 directionalInput = movementInput;
        float x = directionalInput.x;
        float y = directionalInput.y;

        transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

        Vector3 worldSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -worldSize.x, worldSize.x),
            Mathf.Clamp(transform.position.y, -worldSize.y, worldSize.y),
            transform.position.z);
    }
}
