using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Inputs input;

    public int player;
    public bool cpu;
    public int dif;
    public GameObject difficulty;

    public bool inactive;

    public bool entered;
    public int damage;
    public int spawnDamage;
    public int score;
    public bool end;

    public static List<Character> characters = new List<Character>();
    public static List<int> players = new List<int>();
    public static List<bool> cpus = new List<bool>();
    public static List<int> diffy = new List<int>();

    public static List<Character> clones = new List<Character>();
    public static List<Player> allPlayers = new List<Player>();

    public static bool sd;

    public CameraController camera;

    public SpriteRenderer render;
    public Color color;

    public SpriteRenderer selecter;
    public Color selecterColor;

    public Roster roster;
    public Roster Kroster;
    public static CharacterSelect select;
    public CharacterStats stats;

    public Character character;

    public Transform respawn;

    public Player hit;

    void Start()
    {
        roster = FindObjectOfType<Roster>();

        if (GameManager.gamemode == 1)
        {
            if(!GameManager.clones)
                score = GameManager.stocks;
        }

        foreach (int p in players)
        {
            if (players.IndexOf(p) == player - 1)
            {
                foreach (Character c in characters)
                {
                    if (characters.IndexOf(c) == p)
                    {
                        this.character = c;
                        Transform character = c.rig;

                        if (transform.localScale.x == -1)
                        {
                            Vector3 scale = transform.localScale;

                            transform.localScale = new Vector3(scale.x * -1, scale.y * 1, scale.z * 1);
                            character.transform.localScale = new Vector3(scale.x * 1, scale.y * 1, scale.z * 1);
                        }
                        else
                        {
                            Vector3 scale = transform.localScale;

                            transform.localScale = new Vector3(scale.x * 1, scale.y * 1, scale.z * 1);
                            character.transform.localScale = new Vector3(scale.x * 1, scale.y * 1, scale.z * 1);
                        }

                        foreach (bool u in cpus)
                        {
                            foreach (int d in diffy)
                            {
                                if (cpus.IndexOf(u, player - 1) == player - 1)
                                {
                                    if (diffy.IndexOf(d, player - 1) == player - 1)
                                    {
                                        cpu = u;
                                        dif = d;
                                        stats.ai.dif = d;
                                        stats.cpu = u;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (roster != null)
        {
            foreach (Character c in roster.characters)
            {
                characters.Add(c);
            }

            foreach (Player p in allPlayers)
            {
                allPlayers.Remove(p);
            }

            if (GameManager.clones)
            {
                foreach (Character c in Kroster.characters)
                {
                    clones.Add(c);
                }
            }
        }
    }

    public void Join()
    {
        inactive = false;
        DontDestroyOnLoad(gameObject);
        FindObjectOfType<GameManager>().Go();
        sd = false;
        return;
    }

    void Update()
    {
        if (roster == null)
        {
            if(inactive)
            {
                foreach (int p in players)
                {
                    players.Remove(p);
                }
            }
            else
            {
                Scene currentScene = SceneManager.GetActiveScene();
                if(currentScene.name == "Victory")
                {
                    var victor = FindObjectOfType<Victory>();

                    foreach(Player p in allPlayers)
                    {
                        if (p.player != player)
                        {
                            if(!end)
                            {
                                stats.transform.localPosition = new Vector3(0, 0, 0);

                                if (allPlayers[player - 1].score > p.score)
                                {
                                    sd = true;
                                    stats.anim.Play("Victory");
                                    transform.position = new Vector3(0, 0, 0);
                                    victor.quit.enabled = true;
                                    victor.text.text = character.name + " Wins!";
                                }
                                else
                                {
                                    if(sd)
                                    {
                                        stats.anim.Play("Results");

                                        if (player == 1)
                                        {
                                            transform.position = new Vector3(-2, 0, 0);
                                        }

                                        if (player == 2)
                                        {
                                            transform.position = new Vector3(2, 0, 0);
                                        }

                                        if (player == 3)
                                        {
                                            transform.position = new Vector3(-1, 0, 0);
                                        }

                                        if (player == 4)
                                        {
                                            transform.position = new Vector3(1, 0, 0);
                                        }
                                    }

                                    end = true;
                                }


                            }

                            if(sd)
                            {
                                if (allPlayers[player - 1].score > p.score)
                                {
                                    victor.quit.enabled = true;
                                    victor.text.text = character.name + " Wins!";
                                }
                            }
                        }
                    }
                }
                if (currentScene.name == "Menu" || currentScene.name == "Roster" || currentScene.name == "RuleSet" || currentScene.name == "Tourney")
                {
                    Destroy(gameObject);
                    stats.End();

                    foreach (Player p in allPlayers)
                    {
                    allPlayers.Remove(p);
                    }
                }
            }
        }

        if (!inactive)
        {
            if (stats == null)
            {
                Scene currentScene = SceneManager.GetActiveScene();
                if (currentScene.name == "Victory")
                {
                    sd = true;
                    Destroy(gameObject);
                }

                if (GameManager.clones)
                {
                    if (player > 1)
                    {
                        SpawnClone();
                        hit.score++;
                    }
                }
                else
                if (GameManager.versus)
                {
                    if (GameManager.gamemode == 0)
                    {
                        if (entered)
                        {
                            score--;
                            Respawn();
                            hit.score++;
                        }
                        else
                        {
                            Respawn();
                        }
                    }

                    if (GameManager.gamemode == 1)
                    {
                        if (entered)
                        {
                            score--;
                            if (score <= 0)
                            {
                                Destroy(gameObject);
                            }
                            else
                            {
                                Respawn();
                            }
                        }
                        else
                        {
                            Respawn();
                        }
                    }
                }
                else
                {
                    Respawn();
                }

            }

            if (transform.childCount > 1)
            {
                foreach (Transform child in transform)
                {
                    int index = child.GetSiblingIndex();
                    if (index < 2)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }

        if (!cpu)
        {
            render.color = color;

            if (gameObject.GetComponentInChildren<PlayerCursor>())
            {
                selecter.color = selecterColor;
                difficulty.SetActive(false);
            }
        }

        select = GetComponentInChildren<CharacterSelect>();

        if(select == null)
        {
            return;
        }

        if(select.cpu == true)
        {
            cpu = true;
            dif = difficulty.GetComponent<Dropdown>().value;
        }
        else
        {
            cpu = false;
        }
    }

    public void AddCharacter(int player, int c)
    {
        players.Insert(player - 1, c);
        cpus.Insert(player - 1, cpu);
        diffy.Insert(player - 1, dif);
        if(players.IndexOf(player) != null)
        {
            foreach(Player p in allPlayers)
            {
                players.Insert(p.player - 1, p.GetComponentInChildren<CharacterSelect>().currentCharacter.GetSiblingIndex());
                cpus.Insert(p.player - 1, p.cpu);
                diffy.Insert(p.player - 1, p.dif);
            }
        }

        if(GameManager.clones)
        {
            players.Insert(player, c + 1);
            cpus.Insert(player, true);
            diffy.Insert(player, dif);
        }
    }

    public void RemoveCharacter(int player, int c)
    {
        players.RemoveAt(player - 1);
        cpus.RemoveAt(player - 1);
        diffy.RemoveAt(player - 1);

        if (GameManager.clones)
        {
            players.RemoveAt(player);
            cpus.RemoveAt(player);
            diffy.RemoveAt(player);
        }
    }

    public IEnumerator Spawned()
    {
        yield return new WaitForSeconds(0.1f);
        entered = true;
    }

    public void Respawn()
    {
        damage = spawnDamage;

        if(!entered)
        {
            Transform c = Instantiate(character.rig, transform);
            c = transform.GetChild(0);
            stats = GetComponentInChildren<CharacterStats>();
            StartCoroutine(Spawned());
        }
        else
        {
            Transform c = Instantiate(character.rig, respawn.position, Quaternion.identity, transform);
            c = transform.GetChild(0);
            camera.players.Insert(player - 1, c);
            stats = GetComponentInChildren<CharacterStats>();
        }
    }

    public void SpawnClone()
    {
        damage = spawnDamage;

        character = clones[Random.Range(0, clones.Count)];
        Transform c = Instantiate(character.rig, respawn.position, Quaternion.identity, transform);
        c = transform.GetChild(0);
        camera.players.Insert(player - 1, c);
        stats = GetComponentInChildren<CharacterStats>();
        return;
    }
}
