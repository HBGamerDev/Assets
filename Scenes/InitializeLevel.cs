using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InitializeLevel : MonoBehaviour
{
    public List<Transform> spawns;
    public List<GameObject> characters;
    public List<InputDevice> devices;
    public PlayerInputManager manager;
    public int index;

    void Start()
    {
        StartCoroutine(PlayerCheck());
    }

    public IEnumerator PlayerCheck()
    {
        yield return new WaitForSeconds(0.1f);

        if (characters.Count < spawns.Count)
        {
            spawns.RemoveAt(spawns.Count - 1);
        }
        StartCoroutine(PlayerCheck());
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform p in spawns)
        {
            if (!characters.Contains(p.GetComponent<Player>().character.rig.gameObject))
            characters.Add(p.GetComponent<Player>().character.rig.gameObject);

            if(index == spawns.IndexOf(p))
            {
                if (p.GetComponent<Player>().cpu)
                {
                    StartCoroutine(Join(p));
                }
            }
        }

        if (index == characters.Count)
        {
            manager.playerPrefab = null;
            foreach(Transform spawn in spawns)
            {
                spawn.GetComponent<Player>().Join();
            }
        }

        manager.playerPrefab = characters[index];
    }

    public IEnumerator Join(Transform p)
    {
        yield return new WaitForSeconds(0.1f);
        p.GetComponent<Player>().Join();
    }

    public void PlayerJoin(PlayerInput input)
    {
        for(int i = 0; i < characters.Count - 1; i++)
        {
            input.transform.position = spawns[index].position;
            input.transform.SetParent(spawns[index]);
        }
    }
}
