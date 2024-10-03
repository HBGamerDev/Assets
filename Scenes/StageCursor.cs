using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class StageCursor : MonoBehaviour
{
    public GraphicRaycaster ray;
    public PointerEventData ped = new PointerEventData(null);
    public Transform stage;

    public StageSelect select;

    public PlayerInput input;

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

    // Start is called before the first frame update
    void Start()
    {
        select.ShowStage(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (stage != null)
            {
                select.Play(select.stages[stage.GetSiblingIndex()]);
            }
        }

        ray = FindObjectOfType<GraphicRaycaster>();
        stage = transform.Find("stage display");
        select = FindObjectOfType<StageSelect>();

        ped.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        ray.Raycast(ped, results);

        if (results.Count > 0)
        {
            Transform rayC = results[0].gameObject.transform;

            if (rayC != stage)
            {
                SetStage(rayC);
            }
        }
        else
        {
            if (stage == null)
            {
                SetStage(null);
            }
        }
    }

    void SetStage(Transform t)
    {
        stage = t;

        if (t != null)
        {
            int index = t.GetSiblingIndex();
            Stage stage = select.stages[index];
            select.ShowStage(stage);
        }
        else
        {
            select.ShowStage(null);
        }
    }
}
