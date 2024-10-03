using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    public List<Stage> stages = new List<Stage>();

    public GameObject cellPrefab;
    public GameObject list;
    public GameObject cursor;
    public GameObject display;

    void Start()
    {
        foreach (Stage stage in stages)
        {
            SpawnCell(stage);
        }

        cursor = FindObjectOfType<StageCursor>().gameObject;
        cursor.transform.parent = transform;
    }

    private void SpawnCell(Stage stage)
    {
        GameObject cell = Instantiate(cellPrefab, list.transform);

        Image screenshot = cell.transform.Find("Stage").GetComponent<Image>();

        screenshot.sprite = stage.stage;
        cell.name = stage.name;
    }

    public void ShowStage(Stage stage)
    {
        bool nullStage = (stage == null);

        Sprite screenshot = nullStage ? null : stage.stage;

        string name = nullStage ? string.Empty : stage.name;

        Transform slot = display.transform;

        slot.Find("display").GetComponent<Image>().sprite = screenshot;
        slot.Find("name").GetComponent<Text>().text = name;
    }

    public void Play(Stage stage)
    {
        SceneManager.LoadScene(stage.name);
        GameManager.tourneyRotation++;
    }
}
