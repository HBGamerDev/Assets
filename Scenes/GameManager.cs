using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool solo;
    public static bool versus;
    public static bool files;

    public static bool training;
    public static bool clones;

    public static bool tourney;
    public Text playerText;
    public static int tourneyPlayers = 8;
    public static bool tourneyStart;
    public static int tourneyRotation;
    public static bool extraPlayer;

    public GameObject rules;
    public GameObject bracket;

    public static bool songlist;
    public static bool voices;

    public Transform pausedMenu;
    public Camera camera;

    public bool paused;

    public PlayerData[] players;

    public GameObject combo;

    public Text gamemodeText;
    public List<string> modes;
    public static int gamemode = 1;

    public Text timeText;
    public float seconds;
    public static int minutes;
    public static int setMinute = 2;

    public Text stocksText;
    public static int stocks = 3; 

    public GameObject trainingMenu;
    public GameObject battle;
    public GameObject vault;

    public Text damageText;
    public int damage;

    public Text fixText;
    public bool fixedDamage;

    public Text hitText;
    public bool hitboxes;
    public GameObject hit;

    public Text cpuText;
    public Text[] cpu;
    public List<string> states;
    public int currentState;

    public bool countdown;
    public bool go;
    public float countdownTime = 3;
    public Text cdText;

    Inputs inputs;

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
    }

    void Start()
    {
        players = transform.root.gameObject.GetComponentsInChildren<PlayerData>();
        cpu = cpuText.transform.GetComponentsInChildren<Text>();

        foreach (Text c in cpu)
        {
            states.Add(c.text);
        }
        states.RemoveAt(0);
    }

    public void Go()
    {
        if(!countdown)
        {
            cdText.gameObject.SetActive(true);
            countdown = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        inputs.Player.Pause.performed += _ => Pause();

        if (countdownTime <= 0)
        {
            go = true;
            cdText.gameObject.SetActive(false);
        }
        if (countdown)
        {
            countdownTime -= Time.deltaTime;
            cdText.text ="" + (countdownTime + 1);
        }

        Scene currentScene = SceneManager.GetActiveScene();
        if (solo)
        {
            if (currentScene.name == "Menu" || currentScene.name == "Roster" || currentScene.name == "RuleSet" || currentScene.name == "Tourney")
            {
                camera.backgroundColor = new Color32(0x32, 0x32, 0, 0);
            }

            if (training)
            {
                trainingMenu.SetActive(true);
                combo.SetActive(true);
            }
            else
            {
                if(currentScene.name != "Menu" && currentScene.name != "Roster" && currentScene.name != "RuleSet" && currentScene.name != "Tourney")
                battle.SetActive(true);
            }
        }

        if (versus)
        {
            if(go)
            {
                if(setMinute != 0)
                {
                    timeText.gameObject.SetActive(true);
                    timeText.text = minutes + ":" + seconds;
                    seconds -= Time.deltaTime;
                    if (seconds <= 0)
                    {
                        seconds = 60;
                        minutes--;
                        if (minutes < 0)
                        {
                            SceneManager.LoadScene("Victory");
                        }
                    }
                }
                battle.SetActive(true);
            }

            if (currentScene.name == "Menu" || currentScene.name == "Roster" || currentScene.name == "RuleSet" || currentScene.name == "Tourney")
            {
                camera.backgroundColor = new Color32(0x32, 0, 0, 0);
                minutes = setMinute;
            }
        }

        if (files)
        {
            camera.backgroundColor = new Color32(0x32, 0, 0x32, 0);
        }

        if (playerText != null)
        {
            playerText.text = "" + tourneyPlayers;
        }

        if (gamemodeText != null)
        {
            if(gamemode == 0)
            {
                gamemodeText.text = "Time";
                stocksText.transform.parent.parent.gameObject.SetActive(false);
            }

            if (gamemode == 1)
            {
                gamemodeText.text = "Stock";
                stocksText.transform.parent.parent.gameObject.SetActive(true);
            }
        }

        if (timeText != null)
        {
            if(setMinute != 0)
            {
                timeText.text = minutes + ":" + seconds;
                if (seconds < 10)
                {
                    timeText.text = minutes + ":0" + seconds;
                }
            }
            else
            {
                timeText.text = "%";
            }
        }

        if (stocksText != null)
        {
            stocksText.text = ""+ stocks;
        }

        if(damageText != null)
        {
            damageText.text = "" + damage;
        }

        if (fixedDamage)
        {
            foreach (PlayerData c in players)
            {
                c.p.damage = damage;
                c.p.spawnDamage = damage;
            }
        }

        if (hitboxes)
        {
            foreach (PlayerData p in players)
            {
                if (p.p.stats.still)
                {
                    p.p.stats.color = new Color32(0xff, 0, 0, 0xff);
                }

                if (p.p.stats.armor)
                {
                    p.p.stats.color = new Color32(0, 0xff, 0, 0xff);
                }

                if (p.p.stats.invincible)
                {
                    p.p.stats.color = new Color32(0, 0, 0xff, 0xff);
                }

                if (!p.p.stats.still && !p.p.stats.armor && !p.p.stats.invincible)
                {
                    p.p.stats.color = new Color32(0xff, 0xff, 0xff, 0xff);
                }

                foreach (HitBox b in p.p.stats.hitboxes)
                {
                    if(b.enabled)
                    Instantiate(hit, b.transform);
                }
            }
        }

        foreach (string cpu in states)
        {
            if(states.IndexOf(cpu) == currentState)
            {
                cpuText.text = "" + cpu;

                foreach(PlayerData p in players)
                {
                    if(training)
                    {
                        if (currentState == 0)
                        {
                            p.p.stats.gameObject.GetComponent<CPU>().enabled = false;
                        }

                        if (currentState == 1)
                        {
                            p.p.stats.gameObject.GetComponent<CPU>().enabled = true;
                        }
                    }
                    else
                    {
                        p.p.stats.gameObject.GetComponent<CPU>().enabled = true;
                    }
                }
            }
        }

        if (tourneyStart)
        {
            rules.SetActive(false);
            bracket.SetActive(true);
            bracket.transform.GetChild(tourneyPlayers).gameObject.SetActive(true);
            foreach (Transform p in bracket.transform.GetChild(tourneyPlayers))
            {
                int index = p.GetSiblingIndex();

                if(tourneyPlayers != p.parent.GetSiblingIndex())
                {
                    p.parent.gameObject.SetActive(false);
                }

                if (tourneyPlayers == 1)
                {
                    if (tourneyRotation == 0)
                    {
                        p.GetComponent<Button>().enabled = true;
                    }

                    if (tourneyRotation == 1)
                    {
                        tourneyRotation = 0;
                        tourneyPlayers = 8;
                        tourney = false;
                        SceneManager.LoadScene("Roster");
                    }

                }

                if (tourneyPlayers == 2)
                {
                    if (tourneyRotation == 0)
                    {
                        p.GetComponent<Button>().enabled = true;
                    }

                    if (tourneyRotation == 1)
                    {
                        tourneyRotation = 0;
                        if(!extraPlayer)
                        {
                            tourneyPlayers = 8;
                            tourney = false;
                            SceneManager.LoadScene("Roster");
                        }
                        else
                        {
                            tourneyPlayers = 1;
                        }
                    }

                }

                if (tourneyPlayers == 4)
                {
                    if(tourneyRotation == 0)
                    {
                        if (index < 2)
                        {
                            p.GetComponent<Button>().enabled = true;
                        }
                    }

                    if (tourneyRotation == 1)
                    {
                        if (index >= 2)
                        {
                            p.GetComponent<Button>().enabled = true;
                        }
                    }

                    if (tourneyRotation == 2)
                    {
                        tourneyRotation = 0;
                        tourneyPlayers = 2;
                    }
                }

                if (tourneyPlayers == 5)
                {
                    tourneyPlayers = 4;
                    extraPlayer = true;
                }

                if (tourneyPlayers == 6)
                {
                    if (tourneyRotation == 0)
                    {
                        if (index < 3)
                        {
                            p.GetComponent<Button>().enabled = true;
                        }
                    }

                    if (tourneyRotation == 1)
                    {
                        if (index >= 3)
                        {
                            p.GetComponent<Button>().enabled = true;
                        }
                    }

                    if (tourneyRotation == 2)
                    {
                        tourneyRotation = 0;
                        tourneyPlayers = 2;
                    }
                }

                if (tourneyPlayers == 7)
                {
                    tourneyPlayers = 6;
                    extraPlayer = true;
                }

                if (tourneyPlayers == 8)
                {
                    if (tourneyRotation == 0)
                    {
                        if (index < 2)
                        {
                            p.GetComponent<Button>().enabled = true;
                        }
                    }

                    if (tourneyRotation == 1)
                    {
                        if (index >= 2 && index < 4)
                        {
                            p.GetComponent<Button>().enabled = true;
                        }
                    }

                    if (tourneyRotation == 2)
                    {
                        if (index >= 4 && index < 6)
                        {
                            p.GetComponent<Button>().enabled = true;
                        }
                    }

                    if (tourneyRotation == 3)
                    {
                        if (index >= 6)
                        {
                            p.GetComponent<Button>().enabled = true;
                        }
                    }

                    if (tourneyRotation == 4)
                    {
                        tourneyRotation = 0;
                        tourneyPlayers = 4;
                    }
                }
            }
        }
    }

    public void Solo()
    {
        solo = true;
        trainingMenu.SetActive(true);
    }

    public void Versus()
    {
        versus = true;
        battle.SetActive(true);
    }

    public void Files()
    {
        files = true;
        vault.SetActive(true);
    }

    public void Training()
    {
        training = true;
    }

    public void Clones()
    {
        clones = true;
    }

    public void Tourney()
    {
        tourney = true;
        tourneyRotation = 0;
        extraPlayer = false;
        SceneManager.LoadScene("Tourney");
    }

    public void SongList()
    {
        songlist = true;
        SceneManager.LoadScene("Songlist");
    }

    public void Voices()
    {
        voices = true;
        SceneManager.LoadScene("Voices");
    }

    public void Menu()
    {
        solo = false;
        versus = false;
        files = false;
        training = false;
        clones = false;
        tourney = false;
        tourneyStart = false;
        songlist = false;
        voices = false;
        SceneManager.LoadScene("Menu");
    }

    public void Rules()
    {
        SceneManager.LoadScene("RuleSet");
    }

    public void NextMode()
    {
        gamemode++;

        if (gamemode > modes.Count - 1)
        {
            gamemode = 0;
        }
    }

    public void PrevMode()
    {
        gamemode--;

        if (gamemode < 0)
        {
            gamemode = modes.Count - 1;
        }
    }

    public void TimeUp()
    {
        setMinute++;

        if (setMinute > 60)
        {
            setMinute = 0;
        }
    }

    public void TimeDown()
    {
        setMinute--;

        if (setMinute < 0)
        {
            setMinute = 60;
        }
    }

    public void StockUp()
    {
        stocks++;

        if (stocks > 99)
        {
            stocks = 1;
        }
    }

    public void StockDown()
    {
        stocks--;

        if (stocks < 1)
        {
            stocks = 99;
        }
    }

    public void PlayerUp()
    {
        tourneyPlayers++;

        if (tourneyPlayers > 8)
        {
            tourneyPlayers = 4;
        }
    }

    public void PlayerDown()
    {
        tourneyPlayers--;

        if (tourneyPlayers < 4)
        {
            tourneyPlayers = 8;
        }
    }

    public void Pause()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name != "Menu" && currentScene.name != "Roster" && currentScene.name != "RuleSet" || currentScene.name == "Tourney")
        {
            if (!paused)
            {
                StartCoroutine(PauseTrigger());
                pausedMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
                return;
            }
            else
            {
                StartCoroutine(PauseTrigger());
                pausedMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
                return;
            }
        }
    }

    public IEnumerator PauseTrigger()
    {
        yield return new WaitForSeconds(1/25);
        if (!paused)
        {
            paused = true;
        }
        else
        {
            paused = false;
        }
    }


    public void DamageUp()
    {
        StartCoroutine(RackUp());
        FixedDamaged();
    }

    public IEnumerator RackUp()
    {
        damage++;

        if (damage > 999)
        {
            damage = 0;
        }

        foreach (PlayerData c in players)
        {
            c.p.damage = damage;
            c.p.spawnDamage = damage;
        }

        yield return new WaitForSeconds(0);
        if (fixedDamage)
           StartCoroutine(RackUp());
    }

    public void DamageDown()
    {
        StartCoroutine(RackDown());
        FixedDamaged();
    }

    public IEnumerator RackDown()
    {
        damage--;

        if (damage < 0)
        {
            damage = 999;
        }

        foreach (PlayerData c in players)
        {
            c.p.damage = damage;
            c.p.spawnDamage = damage;
        }

        yield return new WaitForSeconds(0);
        if (fixedDamage)
            StartCoroutine(RackDown());
    }

    public void FixedDamaged()
    {
        if (!fixedDamage)
        {
            fixedDamage = true;
            fixText.text = "On";
            return;
        }

        if (fixedDamage)
        {
            fixedDamage = false;
            fixText.text = "Off";
            return;
        }
    }

    public void HitBox()
    {
        if (!hitboxes)
        {
            hitboxes = true;
            hitText.text = "On";
            return;
        }

        if (hitboxes)
        {
            hitboxes = false;
            hitText.text = "Off";
            return;
        }
    }

    public void NextState()
    {
        currentState++;

        if(currentState > states.Count - 1)
        {
            currentState = states.Count - 1;
        }
    }

    public void PrevState()
    {
        currentState--;

        if (currentState < 0)
        {
            currentState = 0;
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene("Roster");
        Time.timeScale = 1;

        if(tourney)
        {
            SceneManager.LoadScene("Tourney");
            tourneyStart = true;
            Time.timeScale = 1;
        }
    }

    public void ToBattle()
    {
        SceneManager.LoadScene("Roster");
        Time.timeScale = 1;
    }
}
